
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
                                 <a data-toggle="modal" data-target="#Detail" data-id="${data}" 
                                    class="btn btn-success" style="font-size:small">Detail</a> |
                                </a>
                                <a href="/Screens/Details/${data}" class="btn btn-danger text-white" style="cursor:pointer">
                                    Detail</a>
                                <a onClick=Delete("/Screens/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    Delete</a>
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
            "url": "/Screens/Search/" + value,
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
                                 <a data-toggle="modal" data-target="#Detail" data-id="${data}" 
                                    class="btn btn-success" style="font-size:small">Detail</a> |
                                </a>
                                <a href="/Screens/Details/${data}" class="btn btn-danger text-white" style="cursor:pointer">
                                    Detail</a>
                                <a onClick=Delete("/Screens/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    Delete</a>
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


