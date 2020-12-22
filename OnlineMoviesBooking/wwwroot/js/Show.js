(function ($) {
    $('#dataTable').DataTable({
        "ajax": {
            "url": '/Admin/shows/getall'
        },
        "columns": [
            { "data": "movieName" },
            {
                "data": "poster",
                "render": function (data) {
                    return `
                                <img src="${data}" style="width: 150px;"/>
                            `;
                }
            },
            { "data": "timeStart" },
            { "data": "screenName" },
            { "data": "theaterName" },

            {
                "data": "id",
                "render": function (data) {
                    return `
                                 <div class="text-center" style="display:grid" >
                                    <a href="/Admin/Shows/Edit/${data}" class="btn btn-primary text-white" style="cursor:pointer">
                                        Sửa
                                    </a>
                                     <a href="/Admin/Shows/Details/${data}"
                                        class="btn btn-success" style="font-size:small">Chi tiết</a>
                                    </a>
                                    <a onClick=Delete("/Admin/Shows/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                        Xóa</a>
                                </div>
                                `
                }
            }
        ]
    });
    
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
$('#Search').click(function () {
    var value = $('#select').val();
    var table = $('#dataTable').DataTable();
    //ajax.load(url,data,callback)
    table.clear();
    table.destroy();
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/Shows/Search/" + value,
        },
        "columns": [
            { "data": "movieName" },
            {
                "data": "poster",
                "render": function (data) {
                    return `
                                <img src="${data}" style="width: 150px;"/>
                            `;
                }
            },
            { "data": "timeStart" },
            { "data": "screenName" },
            { "data": "theaterName" },

            {
                "data": "id",
                "render": function (data) {
                    return `
                                 <div class="text-center" style="display:grid" >
                                    <a href="/Admin/Shows/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                        Edit
                                    </a>
                                     <a href="/Admin/Shows/Details/${data}"
                                        class="btn btn-success" style="font-size:small">Details</a>
                                    </a>
                                    <a onClick=Delete("/Admin/Shows/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                        Delete</a>
                                </div>
                                `
                }
            }
        ]
    });
});
$('#Search-Status').click(function () {
    var value = $('#select-status').val();
    console.log(value);
    var table = $('#dataTable').DataTable();
    //ajax.load(url,data,callback)
    table.clear();
    table.destroy();
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/Shows/SearchStatus?status=" + $('#select-status').val(),
        },
        "columns": [
            { "data": "movieName" },
            {
                "data": "poster",
                "render": function (data) {
                    return `
                                <img src="${data}" style="width: 150px;"/>
                            `;
                }
            },
            { "data": "timeStart" },
            { "data": "screenName" },
            { "data": "theaterName" },

            {
                "data": "id",
                "render": function (data) {
                    return `
                                 <div class="text-center" style="display:grid" >
                                    <a href="/Admin/Shows/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                        Edit
                                    </a>
                                     <a href="/Admin/Shows/Details/${data}"
                                        class="btn btn-success" style="font-size:small">Details</a>
                                    </a>
                                    <a onClick=Delete("/Admin/Shows/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                        Delete</a>
                                </div>
                                `
                }
            }
        ]
    });
});


var triggerTabList = [].slice.call(document.querySelectorAll('#myTab a'))
triggerTabList.forEach(function (triggerEl) {
    var tabTrigger = new bootstrap.Tab(triggerEl)

    triggerEl.addEventListener('click', function (event) {
        event.preventDefault()
        tabTrigger.show()
    })
})