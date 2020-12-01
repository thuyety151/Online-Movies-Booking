$(document).ready(function () {
    $('#AccountTable').DataTable();
});


function showModal(id) {

    var modal = $('#DetailAccountModal');
    console.log(1);
    $.ajax({
        method: "GET",
        url: '/Accounts/Get/' + id,
        success: function (data) {
            console.log(data);
            modal.find(imageUrl).val(data.data[0].image)
            modal.find('#name_detail_account').val(data.data[0].name)
            modal.find('#birthdate_detail_account').val(data.data[0].birthdate)
            modal.find('#gender_detail_account').val(data.data[0].gender)
            modal.find('#address_detail_account').val(data.data[0].address)
            modal.find('#sdt_detail_account').val(data.data[0].sdt)
            modal.find('#email_detail_account').val(data.data[0].email)
            modal.find('#password_detail_account').val(data.data[0].password)
            modal.find('#point_detail_account').val(data.data[0].point)
            modal.find('#image_detail_account').val(data.data[0].image)
            //modal.find().val(data.data[0].id)
        }
    })
}

//$('#DetailAccountModal').on('show.bs.modal', function (event) {
//    var button = $(event.relatedTarget) // Button that triggered the modal
//    var ad = button.data('whatever') // Extract info from data-* attributes
//    var modal = $(this)
//    console.log(1)
//    $.ajax({
//        method: "GET",
//        url: '/Accounts/Get/' + ad,
//        success: function (data) {
            
//            modal.find('#name_detail_account').val(data.data.name)
//            modal.find('#birthdate_detail_account').val(data.data.birthdate)
//            modal.find('#gender_detail_account').val(data.data.gender)
//            modal.find('#address_detail_account').val(data.data.address)
//            modal.find('#sdt_detail_account').val(data.data.sdt)
//            modal.find('#email_detail_account').val(data.data.email)
//            modal.find('#password_detail_account').val(data.data.password)
//            modal.find('#point_detail_account').val(data.data.point)
//            modal.find('#image_detail_account').val(data.data.image)
//            //modal.find().val(data.data[0].id)
//        }
//    })



//})


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

