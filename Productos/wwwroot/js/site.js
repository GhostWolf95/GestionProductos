// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ConfirmarEliminacion(id, maestro) {
    $('#confirm').modal('show');
    $("#ModelButtondelete").data("idRegistro", id);
    $("#ModelButtondelete").data("Maestro", maestro);
}

$("#ModelButtondelete").click(function () {
    var id = $(this).data("idRegistro");
    var maestro = $(this).data("Maestro");
    $.ajax({
        type: 'DELETE',
        url: maestro+"/Delete/"+id,
        success: function (data) {
            $.notify("Registro eliminado correctamente", "success");
            setTimeout(function () {
                location.reload();
            }, 2000);            
        },
        error: function (err, ee) {
            $.notify(err.responseText, "error");
        }
    });
})