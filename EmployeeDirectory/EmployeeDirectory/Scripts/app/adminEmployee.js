$(document).ready(function() {
    $('#showCreateModal').click(function() {
        $("#edit.modal .modal-footer button").html("<span class=\"glyphicon glyphicon-ok-sign\"></span> Create");
        $("#edit.modal #Heading").text("Create New Employee");
        $("#edit.modal #hdnEmpId").val('');
        $("#edit.modal .form-control").val('');
    });
    $(".showEditModal").click(function(evt, el) {
        $("#edit.modal .modal-footer button").html("<span class=\"glyphicon glyphicon-ok-sign\"></span> Update");
        $("#edit.modal #Heading").text("Update details");
        var row = $(this).parents('tr');
        var cells = $(row).find('td');
        $("#edit.modal #hdnEmpId").val($(row).data('empid'));
        $("#edit.modal #name").val($(cells[0]).text());
        $("#edit.modal #title").val($(cells[1]).text());
        $("#edit.modal #location").val($(cells[2]).text());
        $("#edit.modal #email").val($(cells[3]).text());
        
        $("#edit.modal #workphone").val($(cells[4]).find("#workphone").val());
        $("#edit.modal #homephone").val($(cells[4]).find("#homephone").val());
        $("#edit.modal #cellphone").val($(cells[4]).find("#cellphone").val());

    });


    $("#edit.modal .modal-footer button").click(function () {
        if (validateEmployee()) {
            $(".modal-body .alert-danger").addClass('hidden');
            if ($("#edit.modal #hdnEmpId").val() == "") {
                createEmployee();
            } else {
                updateEmployee();
            }
        } else {
            $(".modal-body .alert-danger").removeClass('hidden');
        }
    });
    function validateEmployee() {
        return $("#edit.modal .modal-body .form-control:invalid").length <= 0;
    }
    function createEmployee() {
        var phone = new Array();
        phone.push({ Type: 'Work', Number: $("#edit.modal #workphone").val() });
        phone.push({ Type: 'Home', Number: $("#edit.modal #homephone").val() });
        phone.push({ Type: 'Cell', Number: $("#edit.modal #cellphone").val() });

        var jqxhr = $.post("employee/create",
            {
                name: $("#edit.modal #name").val(),
                jobTitle: $("#edit.modal #title").val(),
                location: $("#edit.modal #location").val(),
                email: $("#edit.modal #email").val(),
                phoneNumber: phone
            },
            function(html) {
                $('.modal').modal('hide');
                $('.modal-backdrop').remove();
                $('#search-results').html(html);
            });
    }

    function updateEmployee() {
        var phone = new Array();
        phone.push({ Type: 'Work', Number: $("#edit.modal #workphone").val() });
        phone.push({ Type: 'Home', Number: $("#edit.modal #homephone").val() });
        phone.push({ Type: 'Cell', Number: $("#edit.modal #cellphone").val() });

        var jqxhr = $.post("employee/update",
            {
                id: $("#edit.modal #hdnEmpId").val(),
                name: $("#edit.modal #name").val(),
                jobTitle: $("#edit.modal #title").val(),
                location: $("#edit.modal #location").val(),
                email: $("#edit.modal #email").val(),
                phoneNumber: phone
            },
            function(html) {
                $('.modal').modal('hide');
                $('.modal-backdrop').remove();
                $('#search-results').html(html);
            });
    }

    $('.showDeleteModal').click(function() {
        var row = $(this).parents('tr');
        $("#delete.modal #hdnEmpId").val($(row).data('empid'));
    });
    $("#delete.modal .modal-footer button.btn-success").click(function() {
        var jqxhr = $.post("employee/delete",
            { id: $("#delete.modal #hdnEmpId").val() },
            function(html) {

                $('#search-results').html(html);
                $('#delete.modal').modal('hide');
                $('.modal-backdrop').remove();
            }).always(function() {

        });
    });
    $("#delete.modal .modal-footer button.btn-default").click(function() {
        $('.modal').modal('hide');
    });

    $(".pagination li a").click(function () {
        searchEmployee($(this).data('pagenumber'));
    });

    function searchEmployee(page) {
        var searchBy = $('#searchBy').val();
        var searchTerm = $('#searchTerm').val();

        var jqxhr = $.post("employee/searchresults",
            { searchBy: searchBy, searchTerm: searchTerm, page: page },
            function (html) {
                $('#search-results').html(html);
            });
    }
});