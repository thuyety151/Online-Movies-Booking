
$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": '/Admin/theaters/getall'
        },
        "columns": [
            { "data": "name","width":"20%" },
            { "data": "address", "width": "40%" },
            { "data": "hotline", "width": "20%" },
            {
                "data": "id", "width": "20%",
                "render": function (data) {
                    return `
                             <div class="text-center" >
                                <a href="/Admin/Theaters/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    Sửa
                                </a>
                                <a href="/Admin/Theaters/Details/${data}" 
                                class="btn btn-success" style="font-size:small">Chi tiết</a> 

                                <a onClick=Delete("/Admin/Theaters/Delete/${data}") class="btn btn-danger text-white"
                                style="cursor:pointer">
                                    Xóa</a>
                            </div>                           
                            `
                }
            }
        ]
    });
    


});
$('#Detail').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idMovie = button.data('id') // Extract info from data-* attributes
    var modal = $(this)
    console.log(idDiscount)
    $.ajax({
        method: 'GET',
        url: '/Theaters/Detail/' + idMovie,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            modal.find('#Id').val(data.Id);
            modal.find('#Name').val(data.Name);
            modal.find('#Genre').val(data.Genre);
            modal.find('#Director').val(data.Director);
            modal.find('#Casts').val(data.Casts);
            modal.find('#Rated').val(data.Rated);
            modal.find('#Description').val(data.Description);
            modal.find('#Trailer').val(data.Trailer);
            modal.find('#ReleaseDate').val(data.ReleaseDate);
            modal.find('#ExpirationDate').val(data.ExpirationDate);
            modal.find('#RunningTime').val(data.RunningTime);
            modal.find('#Poster').val(data.Poster);
        }
    })
})


const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function Delete(url) {
    swalWithBootstrapButtons.fire({
        title: 'Bạn có chắc muốn xóa dữ liệu này?',
        text: "Dữ liệu sau khi xóa không thể khôi phục được!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        console.log("data");
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    console.log(data.success);
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
