//$(document).ready(function () {
//    $('#AccountTable').DataTable();
//});

(function ($) {

    $(document).ready(function () {
        console.log("data table account nef");
        $('#dataTable').DataTable({
            "ajax": {
                "url": '/Admin/Accounts/getall'
            },
            "columns": [
                {
                    "data": "image",
                    "render": function (data) {
                        return `
                            <img src="${data}" style="width: 150px;"/>
                        `;
                    }
                },
                { "data": "name", "width": "5%" },
                
                { "data": "idTypesOfUser" },
                { "data": "idTypeOfMember" },
                { "data": "sdt" },
                { "data": "email" },
                { "data": "password" },
                { "data": "point" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                             <div class="text-center" style="display:grid" >
                                <a href="/Admin/Accounts/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    Sửa
                                </a>
                                 <a href="/Admin/Accounts/Details/${data}"
                                    class="btn btn-success" style="font-size:small">Chi tiết</a> 
                                </a>
                                <a onClick=DeleteAccount("/Admin/Accounts/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    Xóa</a>
                            </div>                           
                            `
                    }
                }
            ]
        });
    });
    

})(jQuery);
const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function DeleteAccount(url) {

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        );
                        // $('#AccountTable').DataTable().ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            data.message,
                            'error'
                        )
                    }
                }

            })

        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your record is safe :)',
                'error'
            )
        }
    })
}