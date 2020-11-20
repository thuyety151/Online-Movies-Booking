$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": '/movies/getall'
        },
        "columns": [
            { "data": "name" },
            { "data": "genre" },
            { "data": "director" },
            { "data": "casts" },
            { "data": "rated" },
            { "data": "description" },
            { "data": "trailer" },
            { "data": "releaseDate" },
            { "data": "expirationDate" },
            { "data": "runningtime" },
            { "data": "poster" },
            {
                "data": "id",
                "render": function (data) {
                    console.log(data);
                    return `
                             <div class="text-center" >
                                <a href="/Movies/Create/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>

                                <a onClick=Delete("/Movies/Edit/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i></a>
                                <a href="#" data-target="#Detail" data-toggle="modal" data-id="${data}" 
                                class="btn btn-success" style="font-size:small">Details</a> |
                            </div>                           
                            
                           `
                }
            }
        ]
    });
}); 


