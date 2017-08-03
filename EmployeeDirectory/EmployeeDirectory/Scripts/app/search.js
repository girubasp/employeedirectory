$(document).ready(function() {
    $('.search-panel .dropdown-menu').find('a').click(function(e) {
        e.preventDefault();
        var param = $(this).attr("href").replace("#", "");
        var concept = $(this).text();
        $('.search-panel span#search_concept').text(concept);
        $('.input-group #searchBy').val(param);
    });

    $("#searchEmployee").click(function() {
        var searchBy = $('#searchBy').val();
        var searchTerm = $('#searchTerm').val();
        if(validateSearch()){
            var jqxhr = $.post("employee/searchresults",
                { searchBy: searchBy, searchTerm: searchTerm, page: 0 },
                function(html) {
                    $('#search-results').html(html);
                });
        }

    });

    function validateSearch() {
        var searchBy = $('#searchBy').val();
        var searchTerm = $('#searchTerm').val();
        if (searchBy === "" || searchTerm === "") {
            if (searchTerm === "")
                $('#searchTerm').attr('title', 'Please input a search term')[0].setCustomValidity(false);
            if (searchBy === "")
                $('#searchBy').siblings('.dropdown-toggle').attr('title', 'Please select the field to search by')
                    .css('border-color', 'red');
            return false;
        } else {
            $('#searchTerm').removeAttr('title')[0].setCustomValidity('');
            $('#searchBy').siblings('.dropdown-toggle').removeAttr('title').css('border-color', '');
            return true;
        }
    }
});
