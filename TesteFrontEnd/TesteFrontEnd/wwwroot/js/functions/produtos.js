validaLogin();
$.ajaxSetup({
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': "Bearer " + localStorage.getItem("token")
    }
});

var itens;
 

function carregarProdutos() {   
    $.get(url + "produtos/")
    .done(function (data) {
        itens = data;
        exibirItens()
    })
    .fail(function (data) {
        console.log("Erro: " + data.message);
    });
}


function incluirItenLista(json) {
    
    var novoItem = {
        id: json.id,
        descricao: json.descricao,
        valor: json.valor,
        limitePorComanda: json.limitePorComanda
    }
    itens.push(novoItem);
    exibirItens();
}

function exibirItens() {
    $("#listaProdutos").html("");
    var id;
    for (i = 0; i < itens.length; i++) {
        var myTr = document.createElement("tr")
        for (a in itens[i]) {
            var mytd = document.createElement("td")
            if (a == "valor") {
                itens[i][a] = itens[i][a].toLocaleString('pt-br', { style: 'currency', currency: 'BRL' });
            }
            if (a == "limitePorComanda" && itens[i][a] == undefined) {
                itens[i][a] = "-";
            }
            mytd.innerHTML = itens[i][a]
            myTr.appendChild(mytd)
        }
        var actionTd = document.createElement("td")

        var deletebtn = document.createElement("button")
        deletebtn.innerHTML = "Deletar"
        deletebtn.setAttribute("class", "btn btn-sm btn-danger")
        deletebtn.setAttribute("onclick", "deletar(" + itens[i]['id'] + ")")

        actionTd.appendChild(deletebtn)
        myTr.appendChild(actionTd)
        document.getElementById("listaProdutos").appendChild(myTr)

    }
    $("#produtos").find("input").val("");  
}

function adicionarProduto() {
    var model = $("#produtos").serializeObject();
    if (model.descricao == "") {
        alert("Digite a Descrição do Produto");
        return;
    }
    if (model.valor == "") {
        alert("Digite a Valor do Produto");
        return;
    }
    model.valor = parseFloat(model.valor);
    
    if (model.limitePorComanda == "" || model.limitePorComanda == undefined) {
        delete model.limitePorComanda;
    } else {
        model.limitePorComanda = parseFloat(model.limitePorComanda);
    }

    $.ajax({
        type: "POST",
        url: url + "produtos",
        data: JSON.stringify(model),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        success: function (data) {            
                incluirItenLista(data);
        
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

function deletar(item) {
    $.ajax({
        type: "DELETE",
        url: url + "produtos/"+ item,
        data: '{"id":'+ item +'}',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + localStorage.getItem("token")
        },
        success: function (data) {
            alert("Removido com Sucesso");
            carregarProdutos();
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

$(document).ready(function () {
    carregarProdutos();
});
