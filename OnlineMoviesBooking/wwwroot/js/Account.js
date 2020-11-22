$(document).ready(function () {
    $('#dataTable').DataTable();
});

$('#DetailModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idaccount = button.data('id') // Extract info from data-* attributes
    console.log(idaccount)
    var modal = $(this)
    $.ajax({
        method: "GET",
        url: '/Accounts/Get/' + idaccount,
        success: function (data) {
            console.log(data)
            modal.find('#name_detail_account').val(data.data.name)
            modal.find('#birthdate_detail_account').val(data.data.birthdate)
            modal.find('#gender_detail_account').val(data.data.gender)
            modal.find('#address_detail_account').val(data.data.address)
            modal.find('#sdt_detail_account').val(data.data.sdt)
            modal.find('#email_detail_account').val(data.data.email)
            modal.find('#password_detail_account').val(data.data.password)
            modal.find('#point_detail_account').val(data.data.point)
            modal.find('#image_detail_account').val(data.data.image)

            //modal.find().val(data.data[0].id)
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
function DeleteAccount(url) {
    //Swal.fire({
    //    title: 'Are you sure?',
    //    text: "You won't be able to revert this!",
    //    icon: 'warning',
    //    showCancelButton: true,
    //    confirmButtonColor: '#3085d6',
    //    cancelButtonColor: '#d33',
    //    confirmButtonText: 'Yes, delete it!'
    //}).then((result) => {
    //    if (result.isConfirmed) {
    //        $.ajax({
    //            type: "DELETE",
    //            url: url,
    //            success: function (data) {
    //                if (data.success) {
    //                    swalWithBootstrapButtons.fire(
    //                        'Deleted!',
    //                        data.message,
    //                        'success'
    //                    );
    //                    $('#dataTable').DataTable().ajax.reload();
    //                }
    //                else {
    //                    swalWithBootstrapButtons.fire(
    //                        'Error',
    //                        data.message,
    //                        'error'
    //                    )
    //                }
    //            }

    //        })
    //    }
    //    else if (result.dismiss === Swal.DismissReason.cancel) {
    //        swalWithBootstrapButtons.fire(
    //            'Cancelled',
    //            'Your record is safe :)',
    //            'error'
    //        )
    //    }
    //})
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
                        $('#dataTable').DataTable().ajax.reload();
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
