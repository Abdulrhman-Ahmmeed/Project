
function onAddCopyModalSuccess(row)
{
    showAlertMessage("Successfully");
    $('#Modal').modal('hide');

    $('tbody').prepend(row);
    //حل مشكلة المنيو مش بتظهر لانها بتنزل بعد ما بيكون حمل
    KTMenu.createInstances();
    //changing couter of copies
    var Count = $('#copiesCount');
    var NewCount = parseInt(Count.text()) + 1;
    Count.text(NewCount);

    //The line $('.Js-alert').addClass('d-none'); adds the CSS class d-none to elements with the class Js-alert.
    //  The class d-none is typically used to hide elements by setting their display property to "none".

    //The line $('table').removeClass('d-none'); removes the CSS class d-none from <table> elements,
    //  which would make the tables visible again if they were previously hidden.

    $('.Js-alert').addClass('d-none');
    $('table').removeClass('d-none');
}
function onEditCopyModalSuccess(row) {
    showAlertMessage("Successfully");
    $('#Modal').modal('hide');

    $(updatedRow).replaceWith(row);
    //حل مشكلة المنيو مش بتظهر لانها بتنزل بعد ما بيكون حمل
    KTMenu.createInstances();
}