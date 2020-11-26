$(document).ready(function () {
    $('#MemberTable').DataTable();
});

//--chi tiết thành viên
function showModal(id) {

    var modal = $('#DetailMemberModal');
    console.log(1);
    $.ajax({
        method: "GET",
        url: '/Members/Details/' + id,
        success: function (data) {
            console.log(data)
            modal.find('#name_detail_member').val(data.data[0].name)
            modal.find('#birthdate_detail_member').val(data.data[0].birthdate)
            modal.find('#gender_detail_member').val(data.data[0].gender)
            modal.find('#address_detail_member').val(data.data[0].address)
            modal.find('#sdt_detail_member').val(data.data[0].sdt)
            modal.find('#email_detail_member').val(data.data[0].email)
            modal.find('#password_detail_member').val(data.data[0].password)
            modal.find('#point_detail_member').val(data.data[0].point)
            modal.find('#image_detail_member').val(data.data[0].image)
            //modal.find().val(data.data[0].id)
        }
    })
}
//$('#DetailMemberModal').on('show.bs.modal', function (event) {
//    var button = $(event.relatedTarget) // Button that triggered the modal
//    var idaccount = button.data('id') // Extract info from data-* attributes
//    console.log(idaccount)
//    var modal = $(this)
//    $.ajax({
//        method: "GET",
//        url: '/Accounts/Get/' + idaccount,
//        success: function (data) {
//            console.log(data)
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

//xóa thành viên
const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function DeleteMember(url) {
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
                        //$('#MemberTable').DataTable().ajax.reload(null, true);
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