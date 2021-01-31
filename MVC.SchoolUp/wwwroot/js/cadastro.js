var DivProcessandoCadastro = $("#DivProcessandoCadastro");
var onBegin = function () {
    //DivProcessandoCadastro.removeClass("invisible");
    ProcessandoGravar();
};
var onComplete = function () {
    //DivProcessandoCadastro.addClass("invisible");
    //$(".BtnCadastro").removeAttr('disabled');
    DesativarProcessandoCadastro();
};

function ProcessandoGravar() {
    if ($('#formPrincipal').valid()) {
        ProcessandoCadastro();
    }
}

function ProcessandoCadastro() {
    $(".BtnCadastro").attr('disabled', 'disabled');
    $("#DivProcessandoCadastro").removeClass("invisible");
}

function DesativarProcessandoCadastro() {
    $("#DivProcessandoCadastro").addClass("invisible");
    $(".BtnCadastro").removeAttr('disabled');
}

$(document).ready(function () {
    //$('.nota').inputmask('99,9', { placeholder: '', numericInput: true });
    //$.validator.methods.range = function (value, element, param) {
    //    var globalizedValue = value.replace(",", ".");
    //    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
    //}
    //$.validator.methods.number = function (value, element) { return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value); }
    DesativarProcessandoCadastro();
});