$(document).ready(function () {

    $('#dataTable').DataTable({
        "ajax": {
            "url": '/Admin/bills/getall'
        },
        "columns": [
            { "data": "accountName" },
            { "data": "movieName" },
            { "data": "timeStart" },
            { "data": "row" },
            { "data": "no" },
            { "data": "totalPrice" },
            { "data": "date" },
            { "data": "status" },
            { "data": "code" },

            {
                "data": "idaccount" , "data": "idshow", "data": "idseat",
                "render": function (data) {
                    return `
                             <div class="text-center" >                             
                                <a href="/Admin/Bills/Details/${data}"
                                    class="btn btn-success" style="font-size:small">Details</a>
                                </a>

                                <a onClick=Delete("/Admin/Bills/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    delete</a>
                            </div>                           
                            `
                }
            }
        ]
    });
});
$('#Detail').on('show.bs.modal', function (event) {
    console.log(button.data('idaccount', 'idshow','idseat'));
    var button = $(event.relatedTarget) // Button that triggered the modal
    var bill = button.data('idaccount', 'idshow', 'idseat') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: 'GET',
        url: '/Admin/Movies/Details/' + bill,
        success: function (data) {
            console.log(data);
            modal.find('#IdAccount').val(data.IdAcount);
            modal.find('#IdShow').val(data.IdShow);
            modal.find('#Id_eat').val(data.IdSeat);
            modal.find('#Date').val(data.Date);
            modal.find('#TotalPrice').val(data.TotalPrice);
            modal.find('#Status').val(data.Status);
            modal.find('#Code').val(data.Code);
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

