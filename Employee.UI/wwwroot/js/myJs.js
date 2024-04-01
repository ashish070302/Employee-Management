$(document).ready(function () {
    $('#StateId').attr('disabled', true);
    $('#CityId').attr('disabled', true);
    LoadStates();

    $('#StateId').change(function () {
        var stateId = $(this).val();
        if (stateId > 0) {
            LoadCities(stateId);
        }
        else {
            alert("Select State");
            $('#CityId').attr('disabled', true);
            $('#CityId').empty().append('<option> Select City </option>');
        }
    });
});

function LoadStates() {
    $('#StateId').empty();

    $.ajax({
        url: '/Employee/State',
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                $('#StateId').attr('disabled', false);
                $('#StateId').append('<option> Select State </option>');
                $('#CityId').append('<option> Select City </option>');
                $.each(response, function (i, data) {
                    $('#StateId').append('<option value=' + data.id + '>' + data.name + '</option>');
                });
            }
            else {
                $('#StateId').attr('disabled', true);
                $('#CityId').attr('disabled', true);
                $('#StateId').append('<option> State Not available</option>');
                $('#CityId').append('<option> City Not available</option>');
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function LoadCities(StateId) {
    $('#CityId').empty();
    $('#CityId').attr('disabled', true);

    $.ajax({
        url: '/Employee/City?Id=' + StateId,
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                $('#CityId').attr('disabled', false);
                $('#CityId').append('<option> Select City </option>');
                $.each(response, function (i, data) {
                    $('#CityId').append('<option value=' + data.id + '>' + data.name + '</option>');
                });
            }
            else {
                $('#CityId').attr('disabled', true);
                $('#CityId').append('<option> City Not available</option>');
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}