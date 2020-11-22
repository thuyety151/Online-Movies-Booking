$(document).ready(function () {

    $('#dataTable').DataTable({
        "ajax": {
            "url": '/Screens/getall'
        },
        "columns": [
            { "data": "name" },
            { "data": "nameTheater" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center" >
                                <a href="/Screens/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    Edit
                                </a>
                                 <a href="#" data-target="#Detail" data-toggle="modal" data-id="${data}" 
                                    class="btn btn-success" style="font-size:small">Details</a> |

                                </a>

                                <a onClick=Delete("/Screens/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    Delete</a>
                            </div>                           
                            `
                }
            }
        ]
    });
});
//$('#Detail').on('show.bs.modal', function (event) {
//    var button = $(event.relatedTarget) // Button that triggered the modal
//    var idMovie = button.data('id') // Extract info from data-* attributes
//    var modal = $(this)
//    console.log(idDiscount)
//    $.ajax({
//        method: 'GET',
//        url: 'Movies/Detail/' + idMovie,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (data) {

//            modal.find('#Id').val(data.Id);
//            modal.find('#Name').val(data.Name);
//            modal.find('#Genre').val(data.Genre);
//            modal.find('#Director').val(data.Director);
//            modal.find('#Casts').val(data.Casts);
//            modal.find('#Rated').val(data.Rated);
//            modal.find('#Description').val(data.Description);
//            modal.find('#Trailer').val(data.Trailer);
//            modal.find('#ReleaseDate').val(data.ReleaseDate);
//            modal.find('#ExpirationDate').val(data.ExpirationDate);
//            modal.find('#RunningTime').val(data.RunningTime);
//            modal.find('#Poster').val(data.Poster);
//        }
//    })
//})
$('#Detail').on('show.bs.modal', function (event) {
    console.log(button.data('id'));
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idDiscount = button.data('id') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: 'GET',
        url: '/Movies/Detail/' + idDiscount,
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


