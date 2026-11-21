$(function () {
    const boton = $("#actualizar-prestamos");
    const estado = $("#estado-prestamos");

    boton.on("click", function () {
        boton.prop("disabled", true);
        estado.text("Cargando préstamos…");

        $.get({ url: boton.data("url"), dataType: "html", cache: false })
            .done(function (filas) {
                $("#filas-prestamos").html(filas);
                estado.text("Listado actualizado.");
            })
            .fail(function () {
                estado.text("No se pudo actualizar el listado.");
            })
            .always(function () {
                boton.prop("disabled", false);
            });
    });
});
