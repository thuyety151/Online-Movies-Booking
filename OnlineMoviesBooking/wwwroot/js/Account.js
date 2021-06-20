

(function ($) {

    $(document).ready(function () {
        console.log("data table account nef");
        $('#dataTable').DataTable({
            "ajax": {
                "url": '/Admin/Accounts/getall'
            },
            "columns": [
                {
                    "data": "image",
                    "render": function (data) {
                        return `
                            <img src="${data}" style="width: 150px;"/>
                        `;
                    }
                },
                { "data": "name", "width": "5%" },

                { "data": "idTypesOfUser" },
                { "data": "idTypeOfMember" },
                { "data": "sdt" },
                { "data": "email" },
                { "data": "password" },
                { "data": "point" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                             <div class="text-center" style="display:grid" >
                                <a href="/Admin/Accounts/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    Sửa
                                </a>
                                 <a href="/Admin/Accounts/Details/${data}"
                                    class="btn btn-success" style="font-size:small">Chi tiết</a> 
                                </a>
                                <a onClick=Delete("/Admin/Accounts/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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

function showModal(id) {

    var modal = $('#DetailAccountModal');
    console.log(1);
    $.ajax({
        method: "GET",
        url: '/Admin/Accounts/Get/' + id,
        success: function (data) {
            console.log(data);
            // modal.find(imageUrl).val(data.data[0].image)
            modal.find('#name_detail_account').val(data.data.name)
            modal.find('#birthdate_detail_account').val(data.data.birthdate)
            modal.find('#gender_detail_account').val(data.data.gender)
            modal.find('#address_detail_account').val(data.data.address)
            modal.find('#sdt_detail_account').val(data.data.sdt)
            modal.find('#email_detail_account').val(data.data.email)
            modal.find('#password_detail_account').val(data.data.password)
            modal.find('#point_detail_account').val(data.data.point)
            modal.find('#image_detail_account').attr('src', data.data.image)
            //modal.find().val(data.data[0].id)
        }
    })
}

