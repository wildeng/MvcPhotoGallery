// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#upload-image').on('click', function () {
  var url = $(this).data('url');
  $.get(url).done(function (data) {
    $('.modal-content').html(data);
    $('#modal-container').modal('show');
  });
});

$('.toggle-gallery').on('click', function () {
  var url = $(this).data('url');
  $.get(url).done(function (data) {
    $('.modal-content').html(data);
    $('#modal-container').modal('show');
  });
});