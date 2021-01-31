//
var onBegin = function () {
    ProcessandoGravar();
};
var onComplete = function () {
    //DivProcessandoCadastro.addClass("invisible");
    //$(".BtnCadastro").removeAttr('disabled');
    DesativarProcessandoCadastro();
};

function ProcessandoCadastro() {
    $(".BtnCadastro").attr('disabled', 'disabled');
    $("#DivProcessandoCadastro").removeClass("invisible");
}

function DesativarProcessandoCadastro() {
    $("#DivProcessandoCadastro").addClass("invisible");
    $(".BtnCadastro").removeAttr('disabled');
}
