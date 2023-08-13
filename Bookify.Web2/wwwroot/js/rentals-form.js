// Initialize an array to store selected copies
var selectedCopies = [];
console.log(maxAllowedCopies);

$(document).ready(function () {
    // Event handler for the search button
    $('.js-search').on('click', function (e) {
        e.preventDefault();

        var serial = $('#Value').val();

        if (selectedCopies.find(c=> c.serial == serial)) {
            showAlertMessage("There is another Copy with the same serial");
            return;
        }

        if (selectedCopies.length >= maxAllowedCopies) {
            showAlertMessage(`You can't add more than ${maxAllowedCopies} books`);
            return;
        }

        // If not a duplicate and not 
        //exceeding the limit, submit the form
        $('#SearchForm').submit();
    });
});

function onAddCopySuccess(copy) {
    // Clear the input field after a successful copy addition
    $('#Value').val('');

    var bookId = $(copy).find('.js-copy').data('book-id');

    if (selectedCopies.find(c => c.bookId == bookId))
    {
        showAlertMessage('you cannot add two copy with the same Id ');
        return;
    }

    // Prepend the new copy to the CopiesForm
    $('#CopiesForm').prepend(copy);

    var copies = $('.js-copy');
    // Reset the selectedCopies array
    selectedCopies = [];

    // Loop through all the copies and update the selectedCopies array
    $.each(copies,function (i, input) {
        var $input = $(input);
        var serial = $input.val();
        var id = $input.data('book-id'); // Retrieve the book ID from the data attribute

        selectedCopies.push({ serial: serial, bookId: id });
        $input.attr('name', `selectedCopies[${i}].serial`);
    });

    console.log(selectedCopies);
}
