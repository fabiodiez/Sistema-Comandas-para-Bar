validaLogin();

$.ajaxSetup({
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': "Bearer " + localStorage.getItem("token")
    }
});

var comandas = [];


function carregaComandas() {
    $(".card-comanda-modelo").remove();

    $.get(url + "comandas/")
        .done(function (data) {
            $(data).each(function (i) {
                statusComanda = data[i].statusComanda;
                var divComandaClass = "open";
                var comandaFechada = "<span class='count-fechada' >#Aberta</span >";
                var comandoDetalhes = "detalhesComanda";
                if (statusComanda == 1) {
                    divComandaClass = "closed";
                    comandaFechada = "<span class='count-fechada' >#Fechada</span >";
                    comandoDetalhes = "notaFiscalComanda";
                }
                
                var element = "<div class='col-md-3 card-counter " + divComandaClass + " card-comanda-modelo' onclick='" + comandoDetalhes + "(\"" + data[i].id + "\",\"" + data[i].codigoComanda + "\")'>"
                    + comandaFechada +
                    "      <span class='count-name' > Comanda Nº:</span >" +
                    "      <span class='count-numbers'>" + data[i].codigoComanda +"</span>" +
                    "      <span class='count-name'>Total Parcial</span>" +
                    "      <span class='count-total'>" + convertReal(data[i].subTotal) +"</span>"
                "</div >"
                               
                $(".lstComandas").append(element);
            });
        })
        .fail(function (data) {
            console.log("Erro: " + data.message);
        });
}
function carregarProdutos() {
    $.get(url + "produtos/")
        .done(function (data) {
            $(data).each(function (i) {
                var o = new Option(data[i].descricao.toUpperCase(), data[i].id);          
                $(o).attr("data-valor", data[i].valor);
                $("#ProdutoId").append(o);
            });
        })
        .fail(function (data) {
            console.log("Erro: " + data.message);
        });
}


function abrirComanda() {
    if ($("#codigoComanda").val() == "") {
        alert("Digite o Código da Comanda");
        return;
    }

    if ($("#Qtd").val() == "0" || $("#Qtd").val() == "" )  {
        alert("Digite a Quantidade");
        return;
    }

    if ($("#ProdutoId").val() == "0") {
        alert("Escolha o Produto");
        return;
    }

    var model = {};
    var itemComanda = {};
    itemComanda.ProdutoId = parseInt($("#ProdutoId").val());
    itemComanda.Qtd = parseInt($("#Qtd").val());
    model.itensComanda = [];
    model.codigoComanda = parseInt($("#codigoComanda").val());
    model.itensComanda.push(itemComanda);
    
    $.ajax({
        type: "POST",
        url: url + "comandas",
        data: JSON.stringify(model),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + localStorage.getItem("token")
        },
        success: function (data) {            
            $("#abrirComanda").find("input").val("");
            $("#abrirComanda").find("select").val("0");
            $("#abrirComanda").find("#Qtd").val("1");
            carregaComandas();
            $("#abrirComanda").modal('hide');
        },
        error: function (x, exception) {
            Object.keys(x).forEach(function (k) {
                if (k == "responseText") {
                    response = JSON.parse(x[k]);
                    alert(response.message);
                }
            });
        }
    }); 
}

function incluirProduto() {
    var id = $("#detalheComandaId").val()
    if (id == 0) return;
    if ($("#ProdutoId").val() == "0") {
        alert("Escolha um produto");
        return;
    }
    if ($("#Qtd").val() <= "0") {
        alert("Digite a Quantidade");
        return;
    }
    var model = {};
    var itemComanda = {};
    model.Id = parseInt(id);
    itemComanda.ProdutoId = parseInt($("#ProdutoId").val());
    itemComanda.Qtd = parseInt($("#Qtd").val());
    model.itensComanda = [];
    model.codigoComanda = parseInt($("#codigoComanda").val());
    model.itensComanda.push(itemComanda);

    $.ajax({
        type: "PUT",
        url: url + "comandas/" + id,       
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + localStorage.getItem("token")
        }, data: JSON.stringify(model),
        success: function (data) {
            $("#abrirComanda").find("input").val("");
            $("#abrirComanda").find("select").val("0");
            $("#abrirComanda").find("#Qtd").val("1");
            carregaComandas();
            $("#abrirComanda").modal('hide');
        },
        error: function (x, exception) {
            Object.keys(x).forEach(function (k) {
                if (k == "responseText") {
                    response = JSON.parse(x[k]);
                    alert(response.message);
                }
            });
        }
    }); 
}

function fecharComanda() {
    if (!confirm("Deseja Fechar a comanda?")) {
        return;
    }
    var id = $("#detalheComandaId").val()
    if (id == 0) return;
    $.ajax({
        type: "POST",
        url: url + "comandas/fecharcomanda/"+id,        
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + localStorage.getItem("token")
        },
        success: function (data) {
            $("#abrirComanda").find("input").val("");
            $("#abrirComanda").find("select").val("0");
            $("#abrirComanda").find("#Qtd").val("1");
            carregaComandas();
            $("#abrirComanda").modal('hide');
            notaFiscalComanda(id);
        },
        error: function (x, exception) {
            Object.keys(x).forEach(function (k) {
                if (k == "responseText") {
                    response = JSON.parse(x[k]);
                    alert(response.message);
                }
            });
        }
    }); 
}


function resetarComanda() {
    if (!confirm("Deseja Resetar a comanda?")) {
        return;
    }
    var id = $("#detalheComandaId").val()
    if (id == 0) return;
    $.ajax({
        type: "POST",
        url: url + "comandas/resetarcomanda/" + id,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + localStorage.getItem("token")
        },
        success: function (data) {
            $("#abrirComanda").find("input").val("");
            $("#abrirComanda").find("select").val("0");
            $("#abrirComanda").find("#Qtd").val("1");
            carregaComandas();
            $("#abrirComanda").modal('hide');
        },
        error: function (x, exception) {
            Object.keys(x).forEach(function (k) {
                if (k == "responseText") {
                    response = JSON.parse(x[k]);
                    alert(response.message);
                }
            });
        }
    });
}

function selecionaProduto() {
    if ($("#ProdutoId").val() != "0") {
        valorUnit = $("#ProdutoId :selected").attr("data-valor");            
        $("#ValorUnit").val(convertReal(valorUnit *1));
        $("#totalItem").val(convertReal(valorUnit * $("#Qtd").val()));
    } else {
        $("#ValorUnit").val(convertReal("0.00"));
        $("#totalItem").val(convertReal("0.00"));
    }
}

function atualizaValor() {
    if ($("#ProdutoId").val() == "0") return;
    valorUnit = $("#ProdutoId :selected").attr("data-valor"); 
    $("#totalItem").val(convertReal(valorUnit * $("#Qtd").val()));
}


function modalAbrirComanda() {
    $('#abrirComanda').modal('show');
    $("#codigoComanda").val("");
    $("#codigoComanda").prop("readonly", false);
    $("#modalComandaTitle").text("Abrir Comanda")
    $("#btn-abrirComanda").show();
    $("#btn-incluirProduto").hide();
    $("#btn-resetarComanda").hide();
    $("#btn-fecharComanda").hide();
    $(".divProdutos").hide();
    $("#divSubTotal").hide();

    $("#detalheComandaId").val("0");
}

function detalhesComanda(id,codigoComanda) {
    $("#abrirComanda").modal('show');
    $("#codigoComanda").val(codigoComanda);
    $("#codigoComanda").prop("readonly", true);
    $("#modalComandaTitle").text("Detalhes da Comanda")
    $("#btn-abrirComanda").hide();
    $("#btn-incluirProduto").show();
    $("#btn-resetarComanda").show();
    $("#btn-fecharComanda").show();
    $(".divProdutos").show();
    $("#divSubTotal").show();

    $("#detalheComandaId").val(id);
    carregaDetalhesComanda(id);
    
}

function carregaDetalhesComanda(id) {
    if (id == 0 || id == undefined) return;
    
    $.get(url + "comandas/"+id)
        .done(function (data) {
            $("#listaProdutos").html("");
            var id;
            $(data.itensComanda).each(function (i) {
                var myTr = document.createElement("tr");
                var tdId = document.createElement("td"); 
                tdId.innerHTML = i + 1;
                myTr.appendChild(tdId);

                var Descricao = document.createElement("td"); 
                Descricao.innerHTML = data.itensComanda[i].produto.descricao;
                myTr.appendChild(Descricao);

                var valorUnit = document.createElement("td"); 
                valorUnit.innerHTML = convertReal(data.itensComanda[i].valorUnit);
                myTr.appendChild(valorUnit);

                var qtd = document.createElement("td"); 
                qtd.innerHTML = data.itensComanda[i].qtd;
                myTr.appendChild(qtd);

                var valorTotal = document.createElement("td"); 
                valorTotal.innerHTML =  convertReal(data.itensComanda[i].valorTotal);
                myTr.appendChild(valorTotal);
                
                document.getElementById("listaProdutos").appendChild(myTr);
                 
            });
            $("#subTotal").text(convertReal(data.subTotal));
            $("#produtos").find("input").val("");  
            
        })
        .fail(function (data) {
            console.log("Erro: " + data.message);
        });
}

function exibirComandas(status) {
    if (status == 0) {
        $(".open").show();
        $(".closed").hide();
    }
    if (status == 1) {
        $(".open").hide();
        $(".closed").show();
    }
    if (status == 2) {
        $(".open").show();
        $(".closed").show();
    }
    
}
function convertReal(value) {
    return value.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' })
}

function notaFiscalComanda(id) {
    if (id == 0 || id == undefined) return;

    $.get(url + "comandas/" + id)
        .done(function (data) {
            gerarNotaFiscal(data);
        })
        .fail(function (data) {
            console.log("Erro: " + data.message);
        });
}
//Nota Fiscal
function gerarNotaFiscal(json) {
    $("div[data-title='novoItem']").remove();    
    var itemCupom = $(".item-cupom").last();
    var itemDesconto = $(".item-desconto").last();
    var i = 0;
    $.each(json.itensComanda, function () {
        i++;
        var cloneItem = itemCupom.clone();
        cloneItem.find(".itemNumero").text(i);                
        cloneItem.find(".descricao").html(this.produto.descricao);
        cloneItem.find(".unidade").html(this.qtd + "UN &nbsp; x &nbsp;" + convertReal(this.valorUnit));        
        cloneItem.find(".valorTotal").text(convertReal(this.valorTotal));                
        cloneItem.attr("data-title", "novoItem");
        cloneItem.appendTo(".itens-cupom");
        cloneItem.show();
    })

    $.each(json.promocoesAplicadas, function () {
        i++;
        var cloneDesconto = itemDesconto.clone();
        cloneDesconto.find(".itemNumero").text(i);
        cloneDesconto.find(".titulo").text(this.promocao.titulo);
        cloneDesconto.find(".promocao").text(this.promocao.descricao);        
        cloneDesconto.find(".desconto").text("Desconto: " + convertReal(this.promocao.valorDesconto * -1));
        cloneDesconto.attr("data-title", "novoItem");

        cloneDesconto.appendTo(".itens-desconto");
        cloneDesconto.show();
    })

    $(".sub-total").text(convertReal(json.subTotal));
    totalDescontos = convertReal(json.subTotal - json.valorFinal);
    $(".total-descontos").text(convertReal(totalDescontos));
    $(".valor-final").text(convertReal(json.valorFinal));

    $("#cupomFiscal").modal('show');
}


//Imprimir Cupom
$("#btnImprimir").click(function () {
    $("#btnImprimir").hide();
    html2canvas($("#cupomPrint"), {
        onrendered: function (canvas) {
            theCanvas = canvas;
            //$("#img-out").append(canvas);
            var win = window.open();
            win.document.write("<br><img src='" + canvas.toDataURL() + "'/>");
            setTimeout(function () {
                win.print();

            }, 100);
            $("#btnImprimir").show();

        }
    });
});



$(document).ready(function () {
    carregaComandas();
    carregarProdutos();  
});



