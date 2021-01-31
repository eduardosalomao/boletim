﻿function ProcessandoIndex() {
    $("#DivProcessandoIndex").removeClass("invisible");
}

function DesativarProcessandoIndex() {
    $("#DivProcessandoIndex").addClass("invisible");
}

$(document).ready(function () {
    DesativarProcessandoIndex();
    $('#GridIndex').DataTable({
        "oLanguage": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sDecimal": ",",
            "sThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        },
        "iDisplayLength": 10,
        "order": [],
        dom: 'Bfrtip',
        responsive: true,
        buttons: [
            {
                extend: 'excel',
                title: $("#HdnPrefixoFileName").val(),
                filename: function () {
                    var d = new Date();
                    var n = d.getTime();
                    return $("#HdnPrefixoFileName").val() + '_' + d.getFullYear() + ((d.getMonth() + 1) < 10 ? '0' + (d.getMonth() + 1) : (d.getMonth() + 1)) + ((d.getDate()) < 10 ? '0' + (d.getDate()) : (d.getDate()));
                }
            },
            {
                extend: 'pdf',
                orientation: 'landscape',
                title: $("#HdnPrefixoFileName").val(),
                filename: function () {
                    var d = new Date();
                    var n = d.getTime();
                    return $("#HdnPrefixoFileName").val() + '_' + d.getFullYear() + ((d.getMonth() + 1) < 10 ? '0' + (d.getMonth() + 1) : (d.getMonth() + 1)) + ((d.getDate()) < 10 ? '0' + (d.getDate()) : (d.getDate()));
                }
            }
        ]
    });
});