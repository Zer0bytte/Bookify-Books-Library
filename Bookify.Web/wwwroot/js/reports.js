$(document).ready(function () {
    $('.page-link').on('click', function () {
        var btn = $(this);
        if (btn.parent().hasClass('active')) return;

        var pageNumber = btn.data('page-number');
        $("#PageNumber").val(pageNumber);
        $("#Filters").submit();
    });

    $('.js-date-range').daterangepicker({
        showDropdowns: true,
        autoUpdateInput: false,
        autoApply: true,
        minYear: 2020,
        maxDate: new Date()
    });

    $('.js-date-range').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));
    });

});