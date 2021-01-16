$(document).ready(function () {

    $('#dataTable').DataTable({
        "ajax": {
            "url": '/Admin/bills/getall'
        },
        "columns": [
            { "data": "nameAccount", "width": "15%" },
            { "data": "movieName", "width": "15%" },
            { "data": "screenName", "width": "15%" },
            { "data": "theaterName", "width": "15%" },
            { "data": "timeStart", "width": "15%" },
            { "data": "row", "width": "15%" },
            { "data": "no", "width": "15%" },
            {
                "data": "id", "width": "10%",
                "render": function (data) {
                    return `
                                </a>
                                 <a href="/Admin/Bills/Details/${data}"
                                    class="btn btn-success" style="font-size:small">Chi tiết</a> 

                                </a>
                                <a onClick=Delete("/Admin/Bills/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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
    var idTicket = button.data('id') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: 'GET',
        url: '/Admin/Bills/Details/' + idTicket,
        success: function (data) {
            console.log(data);
            modal.find('#Id').val(data.Id);
            modal.find('#AccountName').val(data.AccountName);
            modal.find('#MovieName').val(data.MovieName);
            modal.find('#ScreenName').val(data.ScreenName);
            modal.find('#TheaterName').val(data.TheaterName);
            modal.find('#TimeStart').val(data.TimeStart);
            modal.find('#Row').val(data.Row);
            modal.find('#No').val(data.No);
            modal.find('#Date').val(data.Date);
            modal.find('#Status').val(data.Status);
            modal.find('#DiscountName').val(data.DiscountName);
            modal.find('#Point').val(data.Point);
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
        title: 'Bạn có chắc muốn xóa vé này?',
        text: "Dữ liệu sau khi xóa sẽ không thể khôi phục lại!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy',
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
                            'Đã xóa!',
                            'Your file has been deleted.',
                            'success'
                        );
                        $('#dataTable').DataTable().ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Lỗi',
                            'Không thể xóa dữ liệu, có lỗi xảy ra',
                            'error'
                        )
                    }
                }

            })

        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Đã hủy thao tác xóa',
                'error'
            )
        }
    })
}


