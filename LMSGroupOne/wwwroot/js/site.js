// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$('[contenteditable]').on('paste', function (event) {
    var $self = $(this);
    setTimeout(function () {
        $self.html($self.text());
    }, 0);
}).on('keypress', function (event) {
    console.log(event.target.id);
    event.target.contentEditable = false;
    return event.which != 13;
}).on("blur", function (event) { console.log("lost focus") });





