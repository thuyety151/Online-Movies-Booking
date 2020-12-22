(function ($) {

    $(document).ready(function () {

        $('#dataTable').DataTable({
            "ajax": {
                "url": '/Admin/discounts/getall'
            },
            "columns": [
                { "data": "name", "width": "20%" },
                { "data": "code", "width": "5%" },
                {
                    "data": "imageDiscount",
                    "render": function (data) {
                        return `
                            <img src="${data}" style="width: 150px;"/>
                        `;
                    }
                },
                { "data": "dateStart" },
                { "data": "dateEnd" },
                { "data": "used" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                             <div class="text-center" style="display:grid" >
                                <a href="/Admin/Discounts/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    Sửa
                                </a>
                                 <a href="/Admin/Discounts/Details/${data}"
                                    class="btn btn-success" style="font-size:small">Chi tiết</a> 
                                </a>
                                <a onClick=Delete("/Admin/Discounts/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    Xóa</a>
                            </div>                           
                            `
                    }
                }
            ]
        });
    });
    $('#Detail').on('show.bs.modal', function (event) {
        console.log(button.data('id'));
        var button = $(event.relatedTarget) // Button that triggered the modal
        var idDiscount = button.data('id') // Extract info from data-* attributes
        var modal = $(this)
        $.ajax({
            method: 'GET',
            url: '/Admin/Movies/Detail/' + idDiscount,
            success: function (data) {
                console.log(data);
                modal.find('#Id').val(data.Id);
                modal.find('#Name').val(data.Name);
                modal.find('#Genre').val(data.Genre);
                modal.find('#Director').val(data.Director);
                modal.find('#Casts').val(data.Casts);
                modal.find('#Rated').val(data.Rated);
                modal.find('#Description').val(data.Description);
                modal.find('#Trailer').val(data.Trailer);
                modal.find('#ReleaseDate').val(data.ReleaseDate);
                modal.find('#RunningTime').val(data.RunningTime);
                modal.find('#Poster').val(data.Poster);
            }
        })
    })

})(jQuery);
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
function validateInput() {
    console.log("A");
    if (document.getElementById("uploadBox").value = "") {
        console.log("A");
        swal("Error", "Chọn hình ảnh", "error");
        return false;
    }
    return true;
}
