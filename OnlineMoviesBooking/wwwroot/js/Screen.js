
$(document).ready(function () {

    $('#dataTable').DataTable({
        "ajax": {
            "url": '/Admin/Screens/getall'
        },
        "columns": [
            {
                "data": "name", "width": "30%"
            },
            {
                "data": "nameTheater", "width":"30%" },
            {
                "data": "id",
                "width":"40%",
                "render": function (data) {
                    return `
                             <div class="text-center" >
                                <a href="/Admin/Screens/Edit/${data}" class="btn btn-primary text-white" style="cursor:pointer">
                                    Sửa
                                </a>
                                <a href="/Admin/Screens/Details/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    Chi tiết</a>
                                <a onClick=Delete("/Admin/Screens/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    Xóa</a>
                            </div>                           
                            `
                }
            }
        ]
    });
    
});
$('#Detail').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) 
    var recipient = button.data('id')
    console.log(recipient)
});
$('#Search').click(function () {
    var value = $('#select').val();
    console.log(value);
    var table = $('#dataTable').DataTable();
    //ajax.load(url,data,callback)
    table.clear();
    table.destroy();
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/Screens/Search/" + value,
        },
        "columns": [
            {
                "data": "name", "width": "30%"
            },
            {
                "data": "nameTheater", "width":"30%" },
            {
                "data": "id","width":"40%",
                "render": function (data) {
                    return `
                             <div class="text-center" >
                                <a href="/Admin/Screens/Edit/${data}" class="btn btn-primary text-white" style="cursor:pointer">
                                    Sửa
                                </a>
                                <a href="/Admin/Screens/Details/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    Chi tiết</a>
                                <a onClick=Delete("/Admin/Screens/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    Xóa</a>
                            </div>                           
                            `
                }
            }
        ]
    });
});



const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function Delete(url) {

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
                    console.log(data);
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        );
                        $('#dataTable').DataTable().ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            'Can not delete this, maybe it not exit or error from sever',
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


