function mascaraData(campo, e) {
    var kC = (document.all) ? event.keyCode : e.keyCode;
    var data = campo.value;

    if (kC != 8 && kC != 46) {
        if (data.length == 2) {
            campo.value = data += '/';
        }
        else if (data.length == 5) {
            campo.value = data += '/';
        }
        else
            campo.value = data.substr(0,10);
    }
}

function mesAno(campo, e) {
    
    var kC = (document.all) ? event.keyCode : e.keyCode;
    var data = campo.value;

    if (kC != 8 && kC != 46) {
        if (data.length == 2) {
            campo.value = data += '/';
        }
        else if (data.length == 6) {
            campo.value = data;
        }
        else
            campo.value = data.substr(0, 6);
    }
}

function mascaraHora(campo, e) {

    var kC = (document.all) ? event.keyCode : e.keyCode;
    var data = campo.value;

    if (kC != 8 && kC != 46) {
        if (data.length == 2) {
            campo.value = data += ':';
        }
        else if (data.length == 4) {
            campo.value = data;
        }
        else
            campo.value = data.substr(0, 3);
    }
}


function MascaraCNPJ(cnpj, event) {
    if (cnpj.length == 14) {
        if (mascaraInteiro(cnpj) == false) {
            event.returnValue = false;
        }
        return formataCampo(cnpj, '000.000.000-00', event);
    }
    else {
        if (mascaraInteiro(cnpj) == false) {
            event.returnValue = false;
        }
        return formataCampos(cnpj, '00.000.000/0000-00', event);
    }
    
}

//mascara para cep
function MascaraCep(cep, event) {
    if (mascaraInteiro(cep) == false) {
        event.returnValue = false;
    }
    return formataCampo(cep, '00.000-000', event);
}

//mascara para data
function MascaraData(data, event) {
    if (mascaraInteiro(data) == false) {
        event.returnValue = false;
    }
    return formataCampo(data, '00/00/0000', event);
}

//mascara para telefone
function MascaraTelefone(tel, event) {
    if (mascaraInteiro(tel) == false) {
        event.returnValue = false;
    }
    return formataCampo(tel, '(00) 0000-0000', event);
}

function MascaraCelular(tel, event) {
    if (mascaraInteiro(tel) == false) {
        event.returnValue = false;
    }
    return formataCampo(tel, '(00) 00000-0000', event);
}

function MascaraCPF(cnpj, event) {
    if (cnpj.length == 14) {
        if (mascaraInteiro(cnpj) == false) {
            event.returnValue = false;
        }
        return formataCampo(cnpj, '000.000.000-00', event);
    }
    else {
        if (mascaraInteiro(cnpj) == false) {
            event.returnValue = false;
        }
        return formataCampos(cnpj, '00.000.000/0000-00', event);
    }
}

function mascaraInteiro(event) {
    if (event.keyCode < 48 || event.keyCode > 57) {
        event.returnValue = false;
    }
    return true;
}

function formataCampo(campo, Mascara, evento) {

    var boleanoMascara;
    var Digitado = evento.KeyCode;
    exp = /\-|\.|\/|\(|\)| /g;
    campoSoNumeros = campo.value.toString().replace(exp, "");

    var posicaoCampo = 0;
    var NovoValorCampo = "";
    var TamanhoMascara = campoSoNumeros.length;

    if (Digitado != 8) {//backspace
        for (i = 0; i <= TamanhoMascara; i++) {
            boleanoMascara = ((Mascara.charAt(i) == "-") || (Mascara.charAt(i) == "."))
                || (Mascara.charAt(i) == "/");
            boleanoMascara = boleanoMascara || ((Mascara.charAt(i) == "(") || (Mascara.charAt(i) == ")"))
                || (Mascara.charAt(i) == " ");

            if (boleanoMascara) {
                NovoValorCampo += Mascara.charAt(i);
                TamanhoMascara++;
            } else {
                NovoValorCampo += campoSoNumeros.charAt(posicaoCampo);
                posicaoCampo++;
            }
        }
        campo.value = NovoValorCampo;
        return true;
    } else {
        return true;
    }

}

function ValidaTelefone(tel) {
    exp = /\(\d{2}\)\ \d{4}\-d{4}/;
    if (!exp.text(tel.value)) {
        alert("Numero de telefone invalido");
    }
}

//valida CEP
function ValidaCep(cep) {
    exp = /\d{2}\.\d{3}\-\d{3}/;
    if (!exp.test(cep.value))
        alert('Numero de Cep Invalido!');
}

function ValidaData(datap) {
    exp = /\d{2}\/\d{2}\/\d{4}/;
    if (!exp.test(datap.value)) {
        alert("Data invalida!");
    }
}

function ValidaEmail(email) {
    var campo_email = email.value;
    //Checando se o endereço e-mail não esta vazio
    if (campo_email == "") {
        alert("É necessário o preenchimento deste campo.");
        document.id_form.campo_email.focus();
        return false;
    }
    //Checando se o endereço de e-mail é válido
    if (!(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email.value))) {
        alert("É necessário o preenchimento de um endereço de e-mail válido.");
        email.focus();
        return false;
    }
}	