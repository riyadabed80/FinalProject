
//function rsfp_onSelectDate(date, type, args, calendar)
//{
//var dates = args[0];
//    var date = dates[0];
//    var year = date[0], month = date[1], day = date[2];
    
//    var d = new Date(year, month-1, day);
//if (d.getDay() == 0 || d.getDay() == 1) {
//        alert('Please select a date that is different from Monday or Sunday!');
//    return false;
//} else {
//return true;
//    }
//    }
//<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
//<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
//    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
//$(function () {
//    $('#dateselector').datepicker({
//        beforeShowDay: $.datepicker.noWeekends
//    });
//});

$(function () {
    $("#datepicker1, #datepicker2, #datepicker3").datepicker({
        beforeShowDay: $.datepicker.noWeekends, 
        minDate: '-20y',
        maxDate: '0'
    });
});

function total() {
    if ($("#stock1").val + $("#stock2").val + $("#stock3").val == 5000)
    {
        continue;
    }
    else
    {
        alert('Please make sure The Total is $5000');
    }
}