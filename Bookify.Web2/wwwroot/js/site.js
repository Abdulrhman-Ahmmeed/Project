// Shared variables
var table;
var datatable;
var exportedCol = [];
var updatedRow;


const elements = document.getElementsByClassName('ax'); // Replace 'ax' with the appropriate class name

function addClassToElement(targetElement) {
    targetElement.classList.add('active');
}

function removeClassFromElements() {
    for (let i = 0; i < elements.length; i++) {
        elements[i].classList.remove('active');
    }
}

function handleClick() {
    const isActive = this.classList.contains('active');

    if (isActive) {
        this.classList.remove('active');
        localStorage.removeItem('activeElement');
    } else {
        removeClassFromElements();
        addClassToElement(this);
        const elementIndex = Array.from(elements).indexOf(this);
        localStorage.setItem('activeElement', elementIndex);
    }
}
function applySelect2() {
    $('.js-select2').select2();
    $('.js-select2').on('select2:select', function (e) {
        //every element has select2 will stored in this variable 
        var select = $(this);

        $("form").not('#SignOut').validate().element('#' + select.attr('id'))

    });
}

Array.from(elements).forEach((element, index) => {
    element.addEventListener('click', handleClick);

    if (localStorage.getItem('activeElement') === String(index)) {
        addClassToElement(element);
    }
});

function disableSubmitBtn() {
    $('body :submit').attr('disabled', 'disabled').attr('data-kt-indicator', 'on');

}
function onModalBegin() {
    disableSubmitBtn();
}


function showAlertMessage(error) {
    let message = '';

    // Check if the error is an object and contains a responseText property
    if (typeof error === 'object' && error.responseText !== undefined) {
        message = error.responseText;
    } else {
        // If it's not an object or doesn't contain responseText, convert to string
        message = error.toString();
    }

    // Check if the message includes the substring 'Successfully'
    if (message.includes("Successfully")) {
        // Display a success message using Swal.fire
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: message,
            customClass: {
                confirmButton: "btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary"
            }
        });
    } else {
        // Display an error message using Swal.fire
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: message,
            customClass: {
                confirmButton: "btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary"
            }
        });
    }
}


function onModalComplete() {

    $('body :submit').removeAttr('disabled').removeAttr('data-kt-indicator');
}


//data Table

var header = $('th');
$.each(header, function (i) {
    var col = $(this);
    if (!col.hasClass('js-no-exported')) {
        exportedCol.push(i);
    };
});
// Class definition
var KTDatatables = function () {


    // Private functions
    var initDatatable = function () {

        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            'order': [],
            'pageLength': 10,
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $('.js-dataTables').data('document-title');
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCol
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCol
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCol
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCol
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }



    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-dataTables');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();


function onModalSuccess(item) {
    showAlertMessage("Successfully");
    $('#Modal').modal('hide');

    if (updatedRow === undefined) {
        $('tbody').append(item);
    } else {
        $(updatedRow).replaceWith(item);
        updatedRow = undefined;
    }

    KTMenu.init();
    KTMenu.initHandlers();

  
}


$(document).ready(function () {
    //display form to retrive content of Tinymce Editor
    $('form').not('#SignOut').on('submit', function () {

        if ($("#kt_docs_tinymce_basic").length > 0) {
            $("#kt_docs_tinymce_basic").each(function () {

                var input = $(this);
                var content = tinyMCE.get(input.attr('id')).getContent();
                input.val(content);

            });

        }


        var IsValid = $(this).valid();
        if (IsValid) disableSubmitBtn();
    });

    if ($("#kt_docs_tinymce_basic").length > 0) {

        //tinymce
        var options = { selector: "#kt_docs_tinymce_basic", height: "480" };

        if (KTThemeMode.getMode() === "dark") {
            options["skin"] = "oxide-dark";
            options["content_css"] = "dark";
        }

        tinymce.init(options);
    }

    //select2
    applySelect2();



    //dataPicker
    $("#kt_datepicker_1").flatpickr({
/*        maxDate: "15.10.2023"
*/    }
    );

    // Retrieve the text content of the element with ID 'Message'
    var message = $('#Message').text();
    if (message !== '') {
        // Call the showAlertMessage function with the message parameter
        showAlertMessage(message);
    }


    //calling data table 
    KTUtil.onDOMContentLoaded
        (
            function () {
                KTDatatables.init();
            }
        );


    $('body').delegate('.js-rander-model', 'click', function () {
        var btn = $(this);
        var modal = $('#Modal');
        modal.find('#ModalLabel').text(btn.data('title'));
        if (btn.data('update') !== undefined) {
            updatedRow = btn.parents('tr');
            console.log(updatedRow);
        }
        $.get({
            url: btn.data('url'),
            success: function (form) {

                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);

                applySelect2();
            },
            error: function () {
                showAlertMessage();
            }

        });
        modal.modal('show');

    });
    //WE DECLARED IT AS CLASS ON BTN(<a>) TO ACCESS it 
    $('body').delegate('.js-confirm', 'click', function () {
        var btn = $(this);//to bring all element in the clicked row

        bootbox.confirm
            ({
                message: btn.data('message'),
                buttons: {
                    confirm: {
                        label: 'Yes',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'No',
                        className: 'btn-secondary'
                    }
                },
                callback: function (result) {
                    if (result) {
                        $.post
                            ({//TO SPECIFY WHICH CLIKED url i have written in element in _CategoryRow
                                url: btn.data('url'),

                                //PREVENTING CSRF ATTACKS FOR AJAX
                                data:
                                {
                                    '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                                },

                                //LASTUPDATEDON WE HAVE SENDING IT USING TOGGLE ACTION TO CHAGE DATE
                                success: function (LastUpdatedon) {

                                    //JS-STATUS WE HAVE DECLARD IT ON TOGGLED(CHANGED ELEMENT) SPAN
                                    var row = btn.parents('tr');
                                    var status = row.find('.js-status2');
                                    var newStatus = status.text().trim() === 'Locked' ? 'Opend' : 'Locked';
                                    status.text(newStatus).toggleClass('badge-light-success badge-light-danger');
                                    row.find('.js-updated').html(LastUpdatedon);
                                    row.addClass('animate__animated animate__shakeX');
                                    showAlertMessage("Successfully");

                                },
                                error: function () {
                                    showAlertMessage()
                                }
                            });
                    }

                }
            });


    }); //WE DECLARED IT AS CLASS ON BTN(<a>) TO ACCESS it 
    $('body').delegate('.js-toggle-status', 'click', function () {
        var btn = $(this);//to bring all element in the clicked row
        bootbox.confirm
            ({
                message: 'are you sure about deleted this category?',
                buttons: {
                    confirm: {
                        label: 'Yes',
                        className: 'btn-danger'
                    },
                    cancel: {
                        label: 'No',
                        className: 'btn-secondary'
                    }
                },
                callback: function (result) {
                    if (result) {
                        $.post
                            ({//TO SPECIFY WHICH CLIKED url i have written in element in _CategoryRow
                                url: btn.data('url'),

                                //PREVENTING CSRF ATTACKS FOR AJAX
                                data:
                                {
                                    '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                                },

                                //LASTUPDATEDON WE HAVE SENDING IT USING TOGGLE ACTION TO CHAGE DATE
                                success: function (LastUpdatedon) {

                                    //JS-STATUS WE HAVE DECLARD IT ON TOGGLED(CHANGED ELEMENT) SPAN
                                    var row = btn.parents('tr');
                                    var status = row.find('.js-status');
                                    var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                                    status.text(newStatus).toggleClass('badge-light-success badge-light-danger');
                                    row.find('.js-updated').html(LastUpdatedon);
                                    row.addClass('animate__animated animate__shakeX');
                                    showAlertMessage("Successfully");

                                },
                                error: function () {
                                    showAlertMessage()
                                }
                            });
                    }

                }
            });


    });



    $('.js-signOut').on('click', function(){
        $('#SignOut').submit();
    })


});
// @* var elements2 = document.getElementsByClassName('js-toggle-status'); 
        //                             Array.from(elements2).forEach(function (element) {
        //                         element.addEventListener('click', function () {
        //                             var btn = this;
        //                             var id = btn.getAttribute('data-id');

        //                             var xhr = new XMLHttpRequest();
        //                             xhr.open('POST', '/Categories/ToggleStatus/' + id);
        //                               xhr.onload = function () {
        //                                     if (xhr.status === 200) {
        //                                         var status = btn.closest('tr').querySelector('.js-status');
        //                                         var newStatus = status.textContent.trim() === 'Deleted' ? 'Available' : 'Deleted';

        //                                         status.textContent = newStatus;
        //                                         status.classList.toggle('badge-light-success');
        //                                         status.classList.toggle('badge-light-danger');
        //                                     }
        //                                 };

        //                                 xhr.send();
        //                             });
        //                         });  *@