validaLogin();
$.ajaxSetup({
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': "Bearer " + localStorage.getItem("token")
    }
});

var itens;
var Requisitos = [];

function carregarPromocoes() {
    $.get(url + "promocoes/")
        .done(function (data) {
            itens = data;
            exibirItens()
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
                var op = new Option(data[i].descricao.toUpperCase(), data[i].id);                
                $("#ProdutoPromocionalId").append(o);                  
                $("#ProdutoId").append(op);
            });          
        })
        .fail(function (data) {
            console.log("Erro: " + data.message);
        });
}

function incluirRequisito() {
    if ($("#ProdutoId").val() == "0") {
        alert("Escolha um Produto");
        return;
    }
    if ($("#QtdMinima").val() == "") {
        alert("Quantidade Mínima é Obrigatória");
        return;
    }

    var Requisito = new Object();
    Requisito.ProdutoId = parseFloat($("#ProdutoId").val());
    Requisito.QtdMinima = parseFloat($("#QtdMinima").val());
    Requisitos.push(Requisito);
   
    var myTr = document.createElement("tr");
    //var tdId = document.createElement("td");
    var tdProduto = document.createElement("td");
    var tdQtd = document.createElement("td");
    tdProduto.innerHTML = $("#ProdutoId :selected").text()
    tdQtd.innerHTML = $("#QtdMinima").val()
    myTr.appendChild(tdProduto);
    myTr.appendChild(tdQtd);
     
    $("#listaRequisitos").append(myTr);
        
    $("#ProdutoId").val("0");
    $("#QtdMinima").val("");
}

function incluirItenLista(json) {

    var novoItem = {
        id: json.id,
        titulo: json.titulo,
        descricao: json.descricao,
    }
    itens.push(novoItem);
    exibirItens();
}

function exibirItens() {
    $("#listaPromocoes").html("");
    var id;
    for (i = 0; i < itens.length; i++) {
        var myTr = document.createElement("tr")
        for (a in itens[i]) {
            var mytd = document.createElement("td")
            if (a == "id" || a == "titulo" || a == "descricao" ) {            
                mytd.innerHTML = itens[i][a]
                myTr.appendChild(mytd)
            }
        }
        var actionTd = document.createElement("td")

        var deletebtn = document.createElement("button")
        deletebtn.innerHTML = "Deletar"
        deletebtn.setAttribute("class", "btn btn-sm btn-danger")
        deletebtn.setAttribute("onclick", "deletar(" + itens[i]['id'] + ")")

        actionTd.appendChild(deletebtn)
        myTr.appendChild(actionTd)
        document.getElementById("listaPromocoes").appendChild(myTr)

    }
    $("#promocoes").find("input").val("");

}

function adicionarPromocao() {
    if ($("#Titulo").val() == "") {
        alert("Informe o Título da Promoção");
        return;
    }
    if ($("#Descricao").val() == "") {
        alert("Informe a Descrição da Promoção");
        return;
    }
    if ($("#TipoDesconto").val() == "0" && $("#valorDesconto").val() == "") {
        alert("Informe o Valor do Desconto da Promoção");
        return;
    }
    if (Requisitos.length == 0) {
        alert("É Obrigatório incluir ao menos 1(um) requisito para a promoção ");
        return;
    }

    var model = $("#promocoes").serializeObject();
    model.Requisitos = Requisitos;

    model.TipoDesconto = parseFloat(model.TipoDesconto);
    model.valorDesconto = parseFloat(model.valorDesconto);
    model.ProdutoPromocionalId = parseFloat(model.ProdutoPromocionalId);
    //model.TipoDesconto = parseFloat(model.TipoDesconto);

    //if (model.limiteporcomanda == "" || model.limiteporcomanda == undefined) {
    //    delete model.limiteporcomanda;
    //}

    $.ajax({
        type: "POST",
        url: url + "promocoes",
        data: JSON.stringify(model),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        success: function (data) {
           incluirItenLista(data);


        },
        error: function (jqXHR, exception) {
            Object.keys(x).forEach(function (k) {
                if (k == "responseText") {
                    response = JSON.parse(x[k]);
                    alert(response.message);
                }
            });
        }
    });
}

function deletar(item) {
    $.ajax({
        type: "DELETE",
        url: url + "promocoes/" + item,
        data: '{"id":' + item + '}',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        success: function (data) {
            alert("Removido com Sucesso");
            carregarPromocoes();
        },
        error: function (jqXHR, exception) {
            Object.keys(x).forEach(function (k) {
                if (k == "responseText") {
                    response = JSON.parse(x[k]);
                    alert(response.message);
                }
            });
        }
    });
}

//Serializar Form
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

function changeDesconto() {
    if ($("#TipoDesconto").val() == "0") {
        $("#valorDesconto").val("");
        $("#valorDesconto").prop("readonly", false);
    } else {
        $("#valorDesconto").val("0");
        $("#valorDesconto").prop("readonly",true);
    }
}

$(document).ready(function () {
    carregarProdutos();
    carregarPromocoes()
});
