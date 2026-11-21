document.addEventListener("click", function (evento) {
    const enlace = evento.target.closest("a[data-confirmar-eliminacion]");
    if (!enlace || evento.ctrlKey || evento.metaKey || evento.shiftKey || evento.altKey) {
        return;
    }

    evento.preventDefault();
    enlace.closest("form").requestSubmit();
});

document.addEventListener("submit", async function (evento) {
    const formulario = evento.target;
    if (!formulario.matches("form[data-eliminar]")) {
        return;
    }

    evento.preventDefault();

    const pregunta = "¿Deseas eliminar este registro?";
    const confirmado = typeof Swal === "undefined"
        ? window.confirm(pregunta)
        : (await Swal.fire({
            title: pregunta,
            showCancelButton: true,
            confirmButtonText: "Eliminar",
            cancelButtonText: "Cancelar",
            focusCancel: true,
            animation: false,
            buttonsStyling: false,
            customClass: { confirmButton: "boton-dialogo", cancelButton: "boton-dialogo" }
        })).isConfirmed;

    if (confirmado) {
        formulario.submit();
    }
});
