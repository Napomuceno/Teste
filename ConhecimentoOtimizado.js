
function ValidarImportacao()
{
    if (document.getElementById('DdlIdFilial_Filial_XML').value == '') {
        if (document.all)//para IE
            document.getElementById('LblMensagem').innerText = 'Informe a Filial.';
        else // Para FF
            document.getElementById('LblMensagem').textContent = 'Informe a Filial.';

        document.getElementById('LblMensagem').style.color = 'Red';
        document.getElementById('DdlIdFilial_Filial_XML').focus();
        return false;
    }
    if (document.getElementById('DdlTipoNotaXML').value == '') {
        if (document.all)//para IE
            document.getElementById('LblMensagem').innerText = 'Informe o Tipo de Nota.';
        else // Para FF
            document.getElementById('LblMensagem').textContent = 'Informe o Tipo de Nota.';

        document.getElementById('LblMensagem').style.color = 'Red';
        document.getElementById('DdlTipoNotaXML').focus();
        return false;
    }
    if ((document.getElementById('DdlIdNatureza_Natureza_XML').value == '') || (document.getElementById('DdlIdNatureza_Natureza_XML').value == 0)) {
        if (document.all)//para IE
            document.getElementById('LblMensagem').innerText = 'Informe a Natureza';
        else // Para FF
            document.getElementById('LblMensagem').textContent = 'Informe a Natureza.';

        document.getElementById('LblMensagem').style.color = 'Red';
        document.getElementById('DdlIdNatureza_Natureza_XML').focus();
        return false;
    }
    else {
        if ((document.getElementById('DdlIdEspecie_Especie_XML').value == '') || (document.getElementById('DdlIdEspecie_Especie_XML').value == 0)) {
            if (document.all)//para IE
                document.getElementById('LblMensagem').innerText = 'Informe a Espécie.';
            else // Para FF
                document.getElementById('LblMensagem').textContent = 'Informe a Espécie.';

            document.getElementById('LblMensagem').style.color = 'Red';
            document.getElementById('DdlIdEspecie_Especie_XML').focus();
            return false;
        }
        else {
            return true;
        }
    }

}

function CompletaIDCodigoCnpjCpfPagador(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Pagador_ConhecimentoOtimizado').val(str_1);
        $('#TxtNome_Pagador_ConhecimentoOtimizado').val(temp[1]);
        $('#TxtCnpjCpf_Pagador_ConhecimentoOtimizado').val(temp[2]);

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Pagador_ConhecimentoOtimizado').val(temp[1]);
        $('#TxtNome_Pagador_ConhecimentoOtimizado').val(str_1);
        $('#TxtCnpjCpf_Pagador_ConhecimentoOtimizado').val(temp[2]);

    }
    else if ((temp[0].indexOf("Cnpj: ") >= 0) || (temp[0].indexOf("Cpf: ") >= 0)) {

        str = temp[0];
        re = "Cnpj: ";
        str_1 = str.replace(re, "");

        re = "Cpf: ";
        str_1 = str_1.replace(re, "");


        $('#TxtCodCliente_Pagador_ConhecimentoOtimizado').val(temp[2]);
        $('#TxtNome_Pagador_ConhecimentoOtimizado').val(temp[1]);
        $('#TxtCnpjCpf_Pagador_ConhecimentoOtimizado').val(str_1);

    }
}


function Importar()
{
    var botao = document.getElementsByTagName("input");
    
    for (i = 0; i < botao.length; i++) {
        var nome = botao[i].name;
        if (nome.indexOf("BtnNovoImportacaoXML") >= 0)
        {
            botao[i].click();
        }
    }
}

function ListarTabelaFrete()
{
    var botao = document.getElementsByTagName("input");

    for (i = 0; i < botao.length; i++)
    {
        var nome = botao[i].name;
        if (nome.indexOf("BtnTeste") >= 0)
        {
            botao[i].click();
        }
    }
}



function ExecutarEscolhaTela()
{

}

function CompletaIDCodigoCnpjCpfCliente(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Remetente_NotaFiscal').val(Trim(str_1));
        $('#TxtNome_Remetente_NotaFiscal').val(temp[1]);
        $('#TxtCnpjCpf_Remetente_NotaFiscal').val(temp[2]);

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Remetente_NotaFiscal').val(Trim(temp[1]));
        $('#TxtNome_Remetente_NotaFiscal').val(str_1);
        $('#TxtCnpjCpf_Remetente_NotaFiscal').val(temp[2]);

    }
    else if ((temp[0].indexOf("Cnpj: ") >= 0) || (temp[0].indexOf("Cpf: ") >= 0)) {

        str = temp[0];
        re = "Cnpj: ";
        str_1 = str.replace(re, "");

        re = "Cpf: ";
        str_1 = str_1.replace(re, "");


        $('#TxtCodCliente_Remetente_NotaFiscal').val(Trim(temp[2]));
        $('#TxtNome_Remetente_NotaFiscal').val(temp[1]);
        $('#TxtCnpjCpf_Remetente_NotaFiscal').val(str_1);

    }
}


function CompletaIDCodigoCnpjCpfDestinatario(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Destinatario_NotaFiscal').val(Trim(str_1));
        $('#TxtNome_Destinatario_NotaFiscal').val(temp[1]);
        $('#TxtCnpjCpf_Destinatario_NotaFiscal').val(temp[2]);

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Destinatario_NotaFiscal').val(Trim(temp[1]));
        $('#TxtNome_Destinatario_NotaFiscal').val(str_1);
        $('#TxtCnpjCpf_Destinatario_NotaFiscal').val(temp[2]);

    }
    else if ((temp[0].indexOf("Cnpj: ") >= 0) || (temp[0].indexOf("Cpf: ") >= 0)) {

        str = temp[0];
        re = "Cnpj: ";
        str_1 = str.replace(re, "");

        re = "Cpf: ";
        str_1 = str_1.replace(re, "");


        $('#TxtCodCliente_Destinatario_NotaFiscal').val(Trim(temp[2]));
        $('#TxtNome_Destinatario_NotaFiscal').val(temp[1]);
        $('#TxtCnpjCpf_Destinatario_NotaFiscal').val(str_1);

    }
}

function CompletaIDMunicipioColeta(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");
    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioColeta_NotaFiscal').val(Trim(str_1));
        $('#TxtDescMunicipio_MunicipioColeta_NotaFiscal').val(Trim(temp[1]));
        $('#TxtUF_MunicipioColeta_NotaFiscal').val(Trim(temp[2]));

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioColeta_NotaFiscal').val(Trim(temp[1]));
        $('#TxtDescMunicipio_MunicipioColeta_NotaFiscal').val(Trim(str_1));
        $('#TxtUF_MunicipioColeta_NotaFiscal').val(Trim(temp[2]));
    }
}


/* Completa - natureza */
function CompletaIDNaturezaCodDesc(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodNatureza_Natureza_NotaFiscal').val(Trim(str_1));
        $('#TxtDescNatureza_Natureza_NotaFiscal').val(temp[1]);
    }
    else if (temp[0].indexOf("Desc: ") >= 0) {

        str = temp[0];
        re = "Desc: ";
        str_1 = str.replace(re, "");

        $('#TxtCodNatureza_Natureza_NotaFiscal').val(Trim(temp[1]));
        $('#TxtDescNatureza_Natureza_NotaFiscal').val(str_1);
    }
}

function CompletaIDEspecieCodDescr(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodEspecie_Especie_NotaFiscal').val(Trim(str_1));
        $('#TxtDescEspecie_Especie_NotaFiscal').val(temp[1]);
    }
    else if (temp[0].indexOf("Desc: ") >= 0) {

        str = temp[0];
        re = "Desc: ";
        str_1 = str.replace(re, "");

        $('#TxtCodEspecie_Especie_NotaFiscal').val(Trim(temp[1]));
        $('#TxtDescEspecie_Especie_NotaFiscal').val(str_1);
    }
}

function CompletaIDCFOPNota(source, eventArgs) {
//    var descricao = document.getElementById('TxtCodCFOP_CFOP_ProgramacaoDocumento');
//    var fim = new String(eventArgs.get_value()).indexOf("|", 0);
//    descricao.value = new String(eventArgs.get_value()).substring(0, fim);

//    var codigo = document.getElementById('TxtCodCFOP_CFOP_ProgramacaoDocumento');
//    var tamanho = new String(eventArgs.get_value()).length;
//    codigo.value = new String(eventArgs.get_value()).substring(fim + 1, tamanho);
}

function LimpaNatureza(source, eventArgs) {
    var texto = $('#TxtDescNatureza_Natureza_NotaFiscal').val();

    if (Trim(texto) == "") {

        $('#TxtCodNatureza_Natureza_NotaFiscal').val("");
    }
}

function LimpaEspecie(source, eventArgs) {
    var texto = $('#TxtDescEspecie_Especie_NotaFiscal').val();

    if (Trim(texto) == "") {

        $('#TxtCodEspecie_Especie_NotaFiscal').val("");
    }
}

function LimpaLocalColeta(source, eventArgs) {
//    var texto = $('#TxtDescMunicipio_MunicipioColeta_ProgramacaoDocumento').val();

//    if (Trim(texto) == "") {

//        $('#TxtCodMunicipio_MunicipioColeta_ProgramacaoDocumento').val("");
//        $('#TxtUF_MunicipioColeta_ProgramacaoDocumento').val("");
//    }
}

function LimpaLocalEntrega(source, eventArgs) {
//    var texto = $('#TxtDescMunicipio_MunicipioEntrega_ProgramacaoDocumento').val();

//    if (Trim(texto) == "") {

//        $('#TxtCodMunicipio_MunicipioEntrega_ProgramacaoDocumento').val("");
//        $('#TxtUF_MunicipioEntrega_ProgramacaoDocumento').val("");
//    }
}

function ValidarFormularioPreNota() {

//    if (document.getElementById('TxtTotalNF_ProgramacaoDocumento').value == '') {
//        if (document.all)//para IE
//            document.getElementById('LblMensagem').innerText = 'Informe o Total NF.';
//        else // Para FF
//            document.getElementById('LblMensagem').textContent = 'Informe o Total NF.';

//        document.getElementById('LblMensagem').style.color = 'Red';
//        document.getElementById('TxtTotalNF_ProgramacaoDocumento').focus();
//        return false;
//    }


//    if (document.getElementById('TxtCodNatureza_Natureza_ProgramacaoDocumento').value == '') {
//        if (document.all)//para IE
//            document.getElementById('LblMensagem').innerText = 'Informe a Natureza.';
//        else // Para FF
//            document.getElementById('LblMensagem').textContent = 'Informe a Natureza.';

//        document.getElementById('LblMensagem').style.color = 'Red';
//        document.getElementById('TxtDescNatureza_Natureza_ProgramacaoDocumento').focus();
//        return false;
//    }

//    if (document.getElementById('TxtCodEspecie_Especie_ProgramacaoDocumento').value == '') {
//        if (document.all)//para IE
//            document.getElementById('LblMensagem').innerText = 'Informe a Espécie';
//        else // Para FF
//            document.getElementById('LblMensagem').textContent = 'Informe a Espécie';

//        document.getElementById('LblMensagem').style.color = 'Red';
//        document.getElementById('TxtDescEspecie_Especie_ProgramacaoDocumento').focus();
//        return false;
//    }

//    if (document.getElementById('TxtCodMunicipio_MunicipioColeta_ProgramacaoDocumento').value == '') {
//        if (document.all)//para IE
//            document.getElementById('LblMensagem').innerText = 'Informe o Local de Coleta.';
//        else // Para FF
//            document.getElementById('LblMensagem').textContent = 'Informe o Local de Coleta.';

//        document.getElementById('LblMensagem').style.color = 'Red';
//        document.getElementById('TxtDescMunicipio_MunicipioColeta_ProgramacaoDocumento').focus();
//        return false;
//    }
//    if (document.getElementById('TxtCodMunicipio_MunicipioEntrega_ProgramacaoDocumento').value == '') {
//        if (document.all)//para IE
//            document.getElementById('LblMensagem').innerText = 'Informe o Local de Entrega.';
//        else // Para FF
//            document.getElementById('LblMensagem').textContent = 'Informe o Local de Entrega.';

//        document.getElementById('LblMensagem').style.color = 'Red';
//        document.getElementById('TxtDescMunicipio_MunicipioEntrega_ProgramacaoDocumento').focus();
//        return false;
//    }
}

function CompletaIDCodigoCnpjCpfRemetente(source, eventArgs) {
//    var temp = eventArgs.get_value().split("|");

//    if (temp[0].indexOf("Codigo: ") >= 0) {

//        str = temp[0];
//        re = "Codigo: ";
//        str_1 = str.replace(re, "");

//        $('#TxtCodCliente_Remetente_Conhecimento').val(Trim(str_1));
//        $('#TxtNome_Remetente_Conhecimento').val(Trim(temp[1]));
//        $('#TxtCnpjCpf_Remetente_Conhecimento').val(Trim(temp[2]));

//    }
//    else if (temp[0].indexOf("Nome: ") >= 0) {

//        str = temp[0];
//        re = "Nome: ";
//        str_1 = str.replace(re, "");

//        $('#TxtCodCliente_Remetente_Conhecimento').val(Trim(temp[1]));
//        $('#TxtNome_Remetente_Conhecimento').val(Trim(str_1));
//        $('#TxtCnpjCpf_Remetente_Conhecimento').val(Trim(temp[2]));

//    }
//    else if ((temp[0].indexOf("Cnpj: ") >= 0) || (temp[0].indexOf("Cpf: ") >= 0)) {

//        str = temp[0];
//        re = "Cnpj: ";
//        str_1 = str.replace(re, "");

//        re = "Cpf: ";
//        str_1 = str_1.replace(re, "");


//        $('#TxtCodCliente_Remetente_Conhecimento').val(Trim(temp[2]));
//        $('#TxtNome_Remetente_Conhecimento').val(Trim(temp[1]));
//        $('#TxtCnpjCpf_Remetente_Conhecimento').val(Trim(str_1));
//    }
}

function LimpaRemetente(source, eventArgs) {
    var texto = $('#TxtNome_Remetente_NotaFiscal').val();

    if (Trim(texto) == "") {

        $('#TxtCodCliente_Remetente_NotaFiscal').val("");
        $('#TxtNome_Remetente_NotaFiscal').val("");
        $('#TxtCnpjCpf_Remetente_NotaFiscal').val("");
    }
}



/* Completa - Veiculo */
function CompletaIDVeiculo(source, eventArgs) 
{    
    var descricao   = document.getElementById('TxtPlaca_Veiculo_ConhecimentoOtimizado');
    var fim         = new String(eventArgs.get_value()).indexOf("|",0);
    descricao.value = new String(eventArgs.get_value()).substring(0,fim); 
    
    var codigo   = document.getElementById('TxtCodVeiculo_Veiculo_ConhecimentoOtimizado');
    var tamanho  = new String(eventArgs.get_value()).length;
    codigo.value = new String(eventArgs.get_value()).substring(fim + 1,tamanho);
}

function CompletaIDCodigoCnpjCpfRemetenteModal(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_RemetenteModal_Conhecimento').val(Trim(str_1));
        $('#TxtNome_RemetenteModal_Conhecimento').val(Trim(temp[1]));
        $('#TxtCnpjCpf_RemetenteModal_Conhecimento').val(Trim(temp[2]));

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_RemetenteModal_Conhecimento').val(Trim(temp[1]));
        $('#TxtNome_RemetenteModal_Conhecimento').val(Trim(str_1));
        $('#TxtCnpjCpf_RemetenteModal_Conhecimento').val(Trim(temp[2]));

    }
    else if ((temp[0].indexOf("Cnpj: ") >= 0) || (temp[0].indexOf("Cpf: ") >= 0)) {

        str = temp[0];
        re = "Cnpj: ";
        str_1 = str.replace(re, "");

        re = "Cpf: ";
        str_1 = str_1.replace(re, "");


        $('#TxtCodCliente_RemetenteModal_Conhecimento').val(Trim(temp[2]));
        $('#TxtNome_RemetenteModal_Conhecimento').val(Trim(temp[1]));
        $('#TxtCnpjCpf_RemetenteModal_Conhecimento').val(Trim(str_1));
    }

}



function CompletaIDMunicipioLocalColeta(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");
    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioColeta_ConhecimentoOtimizado').val(Trim(str_1));
        $('#TxtDescMunicipio_MunicipioColeta_ConhecimentoOtimizado').val(Trim(temp[1]));
        $('#TxtUF_MunicipioColeta_ConhecimentoOtimizado').val(Trim(temp[2]));

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioColeta_ConhecimentoOtimizado').val(Trim(temp[1]));
        $('#TxtDescMunicipio_MunicipioColeta_ConhecimentoOtimizado').val(Trim(str_1));
        $('#TxtUF_MunicipioColeta_ConhecimentoOtimizado').val(Trim(temp[2]));
    }
}

function CompletaIDMunicipioEntrega(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");
    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioEntrega_NotaFiscal').val(Trim(str_1));
        $('#TxtDescMunicipio_MunicipioEntrega_NotaFiscal').val(Trim(temp[1]));
        $('#TxtUF_MunicipioEntrega_NotaFiscal').val(Trim(temp[2]));

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioEntrega_NotaFiscal').val(Trim(temp[1]));
        $('#TxtDescMunicipio_MunicipioEntrega_NotaFiscal').val(Trim(str_1));
        $('#TxtUF_MunicipioEntrega_NotaFiscal').val(Trim(temp[2]));
    }
}

function CompletaIDCFOPCodDescr(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCFOP_CFOP_NotaFiscal').val(Trim(str_1));
        $('#TxtDescCFOP_CFOP_NotaFiscal').val(temp[1]);
    }
    else if (temp[0].indexOf("Desc: ") >= 0) {

        str = temp[0];
        re = "Desc: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCFOP_CFOP_NotaFiscal').val(Trim(temp[1]));
        $('#TxtDescCFOP_CFOP_NotaFiscal').val(str_1);
    }
}

function LimpaDestinatario(source, eventArgs) {
    var texto = $('#TxtNome_Destinatario_NotaFiscal').val();

    if (Trim(texto) == "") {

        $('#TxtCodCliente_Destinatario_NotaFiscal').val("");
        $('#TxtNome_Destinatario_NotaFiscal').val("");
        $('#TxtCnpjCpf_Destinatario_NotaFiscal').val("");

    }
}

function LimpaLocalColetaNF(source, eventArgs) {
    var texto = $('#TxtDescMunicipio_MunicipioColeta_NotaFiscal').val();

    if (Trim(texto) == "") {

        $('#TxtCodMunicipio_MunicipioColeta_NotaFiscal').val("");
        $('#TxtDescMunicipio_MunicipioColeta_NotaFiscal').val("");
        $('#TxtUF_MunicipioColeta_NotaFiscal').val("");

    }
}

function LimpaLocalEntregaNF(source, eventArgs) {
    var texto = $('#TxtDescMunicipio_MunicipioEntrega_NotaFiscal').val();

    if (Trim(texto) == "") {

        $('#TxtCodMunicipio_MunicipioEntrega_NotaFiscal').val("");
        $('#TxtDescMunicipio_MunicipioEntrega_NotaFiscal').val("");
        $('#TxtUF_MunicipioEntrega_NotaFiscal').val("");

    }
}

function LimparCFOP(source, eventArgs) {
    var texto = $('#TxtDescCFOP_CFOP_NotaFiscal').val();

    if (Trim(texto) == "") {

        $('#TxtCodCFOP_CFOP_NotaFiscal').val("");
    }
}


function LimpaRemetenteConsulta(source, eventArgs) {
    var texto = $('#TxtNome_Remetente_NotaFiscalConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodCliente_Remetente_NotaFiscalConsulta').val("");
        $('#TxtNome_Remetente_NotaFiscalConsulta').val("");
        $('#TxtCnpjCpf_Remetente_NotaFiscalConsulta').val("");
    }
}

function LimpaDestinatarioConsulta(source, eventArgs) {
    var texto = $('#TxtNome_Destinatario_NotaFiscalConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodCliente_Destinatario_NotaFiscalConsulta').val("");
        $('#TxtNome_Destinatario_NotaFiscalConsulta').val("");
        $('#TxtCnpjCpf_Destinatario_NotaFiscalConsulta').val("");

    }
}

function CompletaIDCodigoCnpjCpfClienteConsulta(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Remetente_NotaFiscalConsulta').val(Trim(str_1));
        $('#TxtNome_Remetente_NotaFiscalConsulta').val(temp[1]);
        $('#TxtCnpjCpf_Remetente_NotaFiscalConsulta').val(temp[2]);

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Remetente_NotaFiscalConsulta').val(Trim(temp[1]));
        $('#TxtNome_Remetente_NotaFiscalConsulta').val(str_1);
        $('#TxtCnpjCpf_Remetente_NotaFiscalConsulta').val(temp[2]);

    }
    else if ((temp[0].indexOf("Cnpj: ") >= 0) || (temp[0].indexOf("Cpf: ") >= 0)) {

        str = temp[0];
        re = "Cnpj: ";
        str_1 = str.replace(re, "");

        re = "Cpf: ";
        str_1 = str_1.replace(re, "");


        $('#TxtCodCliente_Remetente_NotaFiscalConsulta').val(Trim(temp[2]));
        $('#TxtNome_Remetente_NotaFiscalConsulta').val(temp[1]);
        $('#TxtCnpjCpf_Remetente_NotaFiscalConsulta').val(str_1);

    }
}

function CompletaIDCodigoCnpjCpfDestinatarioConsulta(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Destinatario_NotaFiscalConsulta').val(Trim(str_1));
        $('#TxtNome_Destinatario_NotaFiscalConsulta').val(temp[1]);
        $('#TxtCnpjCpf_Destinatario_NotaFiscalConsulta').val(temp[2]);

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Destinatario_NotaFiscalConsulta').val(Trim(temp[1]));
        $('#TxtNome_Destinatario_NotaFiscalConsulta').val(str_1);
        $('#TxtCnpjCpf_Destinatario_NotaFiscalConsulta').val(temp[2]);

    }
    else if ((temp[0].indexOf("Cnpj: ") >= 0) || (temp[0].indexOf("Cpf: ") >= 0)) {

        str = temp[0];
        re = "Cnpj: ";
        str_1 = str.replace(re, "");

        re = "Cpf: ";
        str_1 = str_1.replace(re, "");


        $('#TxtCodCliente_Destinatario_NotaFiscalConsulta').val(Trim(temp[2]));
        $('#TxtNome_Destinatario_NotaFiscalConsulta').val(temp[1]);
        $('#TxtCnpjCpf_Destinatario_NotaFiscalConsulta').val(str_1);

    }
}

function LimpaDestinatarioConsultaConsulta(source, eventArgs) {
    var texto = $('#TxtNome_Destinatario_NotaFiscalConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodCliente_Destinatario_NotaFiscalConsulta').val("");
        $('#TxtNome_Destinatario_NotaFiscalConsulta').val("");
        $('#TxtCnpjCpf_Destinatario_NotaFiscalConsulta').val("");

    }
}

function LimpaLocalColetaNFConsulta(source, eventArgs) {
    var texto = $('#TxtDescMunicipio_MunicipioColeta_NotaFiscalConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodMunicipio_MunicipioColeta_NotaFiscalConsulta').val("");
        $('#TxtDescMunicipio_MunicipioColeta_NotaFiscalConsulta').val("");
        $('#TxtUF_MunicipioColeta_NotaFiscalConsulta').val("");

    }
}

function CompletaIDMunicipioColetaConsulta(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");
    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioColeta_NotaFiscalConsulta').val(Trim(str_1));
        $('#TxtDescMunicipio_MunicipioColeta_NotaFiscalConsulta').val(Trim(temp[1]));
        $('#TxtUF_MunicipioColeta_NotaFiscalConsulta').val(Trim(temp[2]));

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioColeta_NotaFiscalConsulta').val(Trim(temp[1]));
        $('#TxtDescMunicipio_MunicipioColeta_NotaFiscalConsulta').val(Trim(str_1));
        $('#TxtUF_MunicipioColeta_NotaFiscalConsulta').val(Trim(temp[2]));
    }
}

function LimpaLocalEntregaNFConsulta(source, eventArgs) {
    var texto = $('#TxtDescMunicipio_MunicipioEntrega_NotaFiscalConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodMunicipio_MunicipioEntrega_NotaFiscalConsulta').val("");
        $('#TxtDescMunicipio_MunicipioEntrega_NotaFiscalConsulta').val("");
        $('#TxtUF_MunicipioEntrega_NotaFiscalConsulta').val("");

    }
}

function CompletaIDMunicipioEntregaConsulta(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");
    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioEntrega_NotaFiscalConsulta').val(Trim(str_1));
        $('#TxtDescMunicipio_MunicipioEntrega_NotaFiscalConsulta').val(Trim(temp[1]));
        $('#TxtUF_MunicipioEntrega_NotaFiscalConsulta').val(Trim(temp[2]));

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioEntrega_NotaFiscalConsulta').val(Trim(temp[1]));
        $('#TxtDescMunicipio_MunicipioEntrega_NotaFiscalConsulta').val(Trim(str_1));
        $('#TxtUF_MunicipioEntrega_NotaFiscalConsulta').val(Trim(temp[2]));
    }
}

function LimpaNaturezaConsulta(source, eventArgs) {
    var texto = $('#TxtDescNatureza_Natureza_NotaFiscalConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodNatureza_Natureza_NotaFiscalConsulta').val("");
    }
}

function LimpaEspecieConsulta(source, eventArgs) {
    var texto = $('#TxtDescEspecie_Especie_NotaFiscalConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodEspecie_Especie_NotaFiscalConsulta').val("");
    }
}

function CompletaIDNaturezaCodDescConsulta(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodNatureza_Natureza_NotaFiscalConsulta').val(Trim(str_1));
        $('#TxtDescNatureza_Natureza_NotaFiscalConsulta').val(temp[1]);
    }
    else if (temp[0].indexOf("Desc: ") >= 0) {

        str = temp[0];
        re = "Desc: ";
        str_1 = str.replace(re, "");

        $('#TxtCodNatureza_Natureza_NotaFiscalConsulta').val(Trim(temp[1]));
        $('#TxtDescNatureza_Natureza_NotaFiscalConsulta').val(str_1);
    }
}

function CompletaIDEspecieCodDescrConsulta(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

       $('#TxtCodEspecie_Especie_NotaFiscalConsulta').val(Trim(str_1));
        $('#TxtDescEspecie_Especie_NotaFiscalConsulta').val(temp[1]);
    }
    else if (temp[0].indexOf("Desc: ") >= 0) {

        str = temp[0];
        re = "Desc: ";
        str_1 = str.replace(re, "");

        $('#TxtCodEspecie_Especie_NotaFiscalConsulta').val(Trim(temp[1]));
        $('#TxtDescEspecie_Especie_NotaFiscalConsulta').val(str_1);
    }
}
function  DesabilitarBotaoSalvar()
{
    document.getElementById('BtnAddComponente').disabled = 'true';
    return true;
}

function ExibeCamporRelacionarViagem() {
    document.getElementById("LblRelacionarViagem").style.display = "block";
    document.getElementById("TxtNumViagem_ConhecimentoOtimizado").style.display = "block";
}

function HiddenCamporRelacionarViagem() {
    document.getElementById("LblRelacionarViagem").style.display = "none";
    document.getElementById("TxtNumViagem_ConhecimentoOtimizado").style.display = "none";
}

function LimpaPagador(source, eventArgs) {
    var texto = $('#TxtNome_Pagador_ConhecimentoOtimizado').val();

    if (Trim(texto) == "") {

        $('#TxtCodCliente_Pagador_ConhecimentoOtimizado').val("");
        $('#TxtCnpjCpf_Pagador_ConhecimentoOtimizado').val("");
    }
}

function LimpaVeiculo(source, eventArgs) {
    var texto = $('#TxtPlaca_Veiculo_ConhecimentoOtimizado').val();

    if (Trim(texto) == "") {
        $('#TxtCodVeiculo_Veiculo_ConhecimentoOtimizado').val("");
    }
}

function AtivaDesativaTotalPellets() {

    var txtTotalPallets = document.getElementById('TxtTotalPallets_ConhecimentoOtimizado');

    var ddlTipoOperacao = document.getElementById('DdlTipoOperacaoTransporte_ConhecimentoOtimizado');

    if (ddlTipoOperacao.value == '1')
        txtTotalPallets.value = '0';

    txtTotalPallets.disabled = (ddlTipoOperacao.value == '1')

    txtTotalPallets.className = ddlTipoOperacao.value == '1' ? 'ctxt' : 'ctxtObr';


}

function CalculaVolumeComposicao() {
    //var TxtAltura = document.getElementById('TxtAlturaComposicao');
    //var TxtLargura = document.getElementById('TxtLarguraComposicao');
    //var TxtComprimento = document.getElementById('TxtComprimentoComposicao');
    //var TxtVolume = document.getElementById('TxtVolumeComposicao');
    //var TxtTarget = document.getElementById('HidTargetControl');
    //var TxtReferencia = document.getElementById('HidReferencia');

    //var altura = getValorCampo(TxtAltura);
    //var largura = getValorCampo(TxtLargura);
    //var comprimento = getValorCampo(TxtComprimento);
    //var volume;

    //if (altura == 0 && largura == 0 && comprimento == 0) {
    //    var TxtVolumeReferencia = document.getElementById(TxtReferencia.value);
    //    volume = getValorCampo(TxtVolumeReferencia);
    //    setValorTextBox(TxtVolume, volume, 4);
    //    return;
    //}

    //if (altura == 0 || largura == 0 || comprimento == 0) {
    //    return;
    //}

    //volume = largura * comprimento * altura;
    //setValorTextBox(TxtVolume, volume, 4);
}

function getValorCampo(campo) {
    var valorRetorno = 0.0;

    if (campo.value.trim() != '')
        valorRetorno = parseFloat(ReplaceAll(ReplaceAll(campo.value, ".", ""), ",", "."));

    return valorRetorno;
}

function setValorTextBox(campo, valor, casasDecimais) {
    campo.value = valor.toFixed(casasDecimais).toString().replace(".", ",");
}

function CalculaVolumeTotalComposicao() {
    //var TxtAltura = document.getElementById('TxtAlturaComposicao');
    //var TxtLargura = document.getElementById('TxtLarguraComposicao');
    //var TxtComprimento = document.getElementById('TxtComprimentoComposicao');
    //var TxtVolume = document.getElementById('TxtVolumeComposicao');
    //var TxtVolumeNota = document.getElementById('HidVolumeTotal');
    //var TxtTarget = document.getElementById('HidTargetControl');
    //var TxtReferencia = document.getElementById('HidReferencia');
    //var TxtFatorCubagem = document.getElementById('TxtFatorCubagenComposicao');

    //if (TxtTarget.value == null || TxtTarget.value != 'TxPTotalPesoCubadoNF_ConhecimentoOtimizado') {
    //    TxtTarget.value = 'TxPTotalPesoCubadoNF_ConhecimentoOtimizado';
    //    TxtReferencia.value = 'HidVolumeTotal'

    //    setValorTextBox(TxtAltura, 0, 4);
    //    setValorTextBox(TxtLargura, 0, 4);
    //    setValorTextBox(TxtComprimento, 0, 4);
    //    setValorTextBox(TxtFatorCubagem, 0, 4);
    //}

    //var altura = getValorCampo(TxtAltura);
    //var largura = getValorCampo(TxtLargura);
    //var comprimento = getValorCampo(TxtComprimento);
    //var volume;

    //if (altura == 0 && largura == 0 && comprimento == 0) {
    //    volume = getValorCampo(TxtVolumeNota);
    //    setValorTextBox(TxtVolume, volume, 4);
    //    return;
    //}

    //if (altura == 0 || largura == 0 || comprimento == 0) {
    //    return;
    //}

    //volume = largura * comprimento * altura;
    //setValorTextBox(TxtVolume, volume, 4);
}

function CalculaVolumeComposicaoNota() {
    //var TxtAltura = document.getElementById('TxtAlturaComposicao');
    //var TxtLargura = document.getElementById('TxtLarguraComposicao');
    //var TxtComprimento = document.getElementById('TxtComprimentoComposicao');
    //var TxtVolume = document.getElementById('TxtVolumeComposicao');
    //var TxtVolumeNota = document.getElementById('TxVVolumeTotal_NotaFiscal');
    //var TxtTarget = document.getElementById('HidTargetControl');
    //var TxtReferencia = document.getElementById('HidReferencia');
    //var TxtFatorCubagem = document.getElementById('TxtFatorCubagenComposicao');

    //if (TxtTarget.value == null || TxtTarget.value != 'TxPPesoCubado_NotaFiscal') {
    //    TxtTarget.value = 'TxPPesoCubado_NotaFiscal';
    //    TxtReferencia.value = 'TxVVolumeTotal_NotaFiscal'

    //    setValorTextBox(TxtAltura, 0, 4);
    //    setValorTextBox(TxtLargura, 0, 4);
    //    setValorTextBox(TxtComprimento, 0, 4);
    //    setValorTextBox(TxtFatorCubagem, 0, 4);
    //}

    //var altura = getValorCampo(TxtAltura);
    //var largura = getValorCampo(TxtLargura);
    //var comprimento = getValorCampo(TxtComprimento);
    //var volume;

    //if (altura == 0 && largura == 0 && comprimento == 0) {
    //    volume = getValorCampo(TxtVolumeNota);
    //    setValorTextBox(TxtVolume, volume, 4);
    //    return;
    //}

    //if (altura == 0 || largura == 0 || comprimento == 0) {
    //    return;
    //}

    //volume = largura * comprimento * altura;
    //setValorTextBox(TxtVolume, volume, 4);
}

function CalculaPesoCubado() {
    //var txtVolume = document.getElementById('TxtVolumeComposicao');
    //var txtFatorCubagem = document.getElementById('TxtFatorCubagenComposicao');
    //var hidTarget = document.getElementById('HidTargetControl');

    //var volume = getValorCampo(txtVolume);
    //var fatorCubagem = getValorCampo(txtFatorCubagem);

    //if (volume > 0 && fatorCubagem > 0) {
    //    var txtTarget = document.getElementById(hidTarget.value);
    //    setValorTextBox(txtTarget, volume * fatorCubagem, 4);
    //}
}
 
function BtnFecharModalTabelaFrete_Click()
{
    var mpe = $find('MpeTabelaFrete');
    if (mpe != null) {
        mpe.hide();
    }
}

function TratarEventoTabelaFreteGeralM03()
{
    var hidTabelaFreteGeralM03 = document.getElementById('HIDPossuiFreteCargaGeralM03');

    if (hidTabelaFreteGeralM03 == null)
        return;

    if (hidTabelaFreteGeralM03.value == '1')
    {
        var botao = document.getElementsByTagName("input");

        for (i = 0; i < botao.length; i++)
        {
            var nome = botao[i].name;
            if (nome.indexOf("BtnTratarTipoOperacao") >= 0) {
                botao[i].click();
                break;
            }
        }
    }
}


function CompletaIDCodigoCnpjCpfEmitenteConsultaCteTerceiro(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Emitente_CteTerceiroConsulta').val(Trim(str_1));
        $('#TxtNome_Emitente_CteTerceiroConsulta').val(temp[1]);
        $('#TxtCnpjCpf_Emitente_CteTerceiroConsulta').val(temp[2]);

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Emitente_CteTerceiroConsulta').val(Trim(temp[1]));
        $('#TxtNome_Emitente_CteTerceiroConsulta').val(str_1);
        $('#TxtCnpjCpf_Emitente_CteTerceiroConsulta').val(temp[2]);

    }
    else if ((temp[0].indexOf("Cnpj: ") >= 0) || (temp[0].indexOf("Cpf: ") >= 0)) {

        str = temp[0];
        re = "Cnpj: ";
        str_1 = str.replace(re, "");

        re = "Cpf: ";
        str_1 = str_1.replace(re, "");


        $('#TxtCodCliente_Emitente_CteTerceiroConsulta').val(Trim(temp[2]));
        $('#TxtNome_Emitente_CteTerceiroConsulta').val(temp[1]);
        $('#TxtCnpjCpf_Emitente_CteTerceiroConsulta').val(str_1);

    }
}

function LimpaEmitenteConsultaCteTerceiro(source, eventArgs) {
    var texto = $('#TxtNome_Emitente_CteTerceiroConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodCliente_Emitente_CteTerceiroConsulta').val("");
        $('#TxtNome_Emitente_CteTerceiroConsulta').val("");
        $('#TxtCnpjCpf_Emitente_CteTerceiroConsulta').val("");

    }
}


function CompletaIDCodigoCnpjCpfClienteConsultaCteTerceiro(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Remetente_CteTerceiroConsulta').val(Trim(str_1));
        $('#TxtNome_Remetente_CteTerceiroConsulta').val(temp[1]);
        $('#TxtCnpjCpf_Remetente_CteTerceiroConsulta').val(temp[2]);

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Remetente_CteTerceiroConsulta').val(Trim(temp[1]));
        $('#TxtNome_Remetente_CteTerceiroConsulta').val(str_1);
        $('#TxtCnpjCpf_Remetente_CteTerceiroConsulta').val(temp[2]);

    }
    else if ((temp[0].indexOf("Cnpj: ") >= 0) || (temp[0].indexOf("Cpf: ") >= 0)) {

        str = temp[0];
        re = "Cnpj: ";
        str_1 = str.replace(re, "");

        re = "Cpf: ";
        str_1 = str_1.replace(re, "");


        $('#TxtCodCliente_Remetente_CteTerceiroConsulta').val(Trim(temp[2]));
        $('#TxtNome_Remetente_CteTerceiroConsulta').val(temp[1]);
        $('#TxtCnpjCpf_Remetente_CteTerceiroConsulta').val(str_1);

    }
}

function LimpaRemetenteConsultaCteTerceiro(source, eventArgs) {
    var texto = $('#TxtNome_Remetente_CteTerceiroConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodCliente_Remetente_CteTerceiroConsulta').val("");
        $('#TxtNome_Remetente_CteTerceiroConsulta').val("");
        $('#TxtCnpjCpf_Remetente_CteTerceiroConsulta').val("");

    }
}


function CompletaIDCodigoCnpjCpfDestinatarioConsultaCteTerceiro(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");

    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Destinatario_CteTerceiroConsulta').val(Trim(str_1));
        $('#TxtNome_Destinatario_CteTerceiroConsulta').val(temp[1]);
        $('#TxtCnpjCpf_Destinatario_CteTerceiroConsulta').val(temp[2]);

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodCliente_Destinatario_CteTerceiroConsulta').val(Trim(temp[1]));
        $('#TxtNome_Destinatario_CteTerceiroConsulta').val(str_1);
        $('#TxtCnpjCpf_Destinatario_CteTerceiroConsulta').val(temp[2]);

    }
    else if ((temp[0].indexOf("Cnpj: ") >= 0) || (temp[0].indexOf("Cpf: ") >= 0)) {

        str = temp[0];
        re = "Cnpj: ";
        str_1 = str.replace(re, "");

        re = "Cpf: ";
        str_1 = str_1.replace(re, "");


        $('#TxtCodCliente_Destinatario_CteTerceiroConsulta').val(Trim(temp[2]));
        $('#TxtNome_Destinatario_CteTerceiroConsulta').val(temp[1]);
        $('#TxtCnpjCpf_Destinatario_CteTerceiroConsulta').val(str_1);

    }
}

function LimpaDestinatarioConsultaCteTerceiro(source, eventArgs) {
    var texto = $('#TxtNome_Destinatario_CteTerceiroConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodCliente_Destinatario_CteTerceiroConsulta').val("");
        $('#TxtNome_Destinatario_CteTerceiroConsulta').val("");
        $('#TxtCnpjCpf_Destinatario_CteTerceiroConsulta').val("");

    }
}


function CompletaIDMunicipioColetaCteTerceiroConsulta(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");
    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioColeta_CteTerceiroConsulta').val(Trim(str_1));
        $('#TxtDescMunicipio_MunicipioColeta_CteTerceiroConsulta').val(Trim(temp[1]));
        $('#TxtUF_MunicipioColeta_CteTerceiroConsulta').val(Trim(temp[2]));

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioColeta_CteTerceiroConsulta').val(Trim(temp[1]));
        $('#TxtDescMunicipio_MunicipioColeta_CteTerceiroConsulta').val(Trim(str_1));
        $('#TxtUF_MunicipioColeta_CteTerceiroConsulta').val(Trim(temp[2]));
    }
}

function LimpaLocalColetaCteTerceiroConsulta(source, eventArgs) {
    var texto = $('#TxtDescMunicipio_MunicipioColeta_CteTerceiroConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodMunicipio_MunicipioColeta_CteTerceiroConsulta').val("");
        $('#TxtDescMunicipio_MunicipioColeta_CteTerceiroConsulta').val("");
        $('#TxtUF_MunicipioColeta_CteTerceiroConsulta').val("");

    }
}


function CompletaIDMunicipioEntregaCTeterceiroConsulta(source, eventArgs) {
    var temp = eventArgs.get_value().split("|");
    if (temp[0].indexOf("Codigo: ") >= 0) {

        str = temp[0];
        re = "Codigo: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioEntrega_CteTerceiroConsulta').val(Trim(str_1));
        $('#TxtDescMunicipio_MunicipioEntrega_CteTerceiroConsulta').val(Trim(temp[1]));
        $('#TxtUF_MunicipioEntrega_CteTerceiroConsulta').val(Trim(temp[2]));

    }
    else if (temp[0].indexOf("Nome: ") >= 0) {

        str = temp[0];
        re = "Nome: ";
        str_1 = str.replace(re, "");

        $('#TxtCodMunicipio_MunicipioEntrega_CteTerceiroConsulta').val(Trim(temp[1]));
        $('#TxtDescMunicipio_MunicipioEntrega_CteTerceiroConsulta').val(Trim(str_1));
        $('#TxtUF_MunicipioEntrega_CteTerceiroConsulta').val(Trim(temp[2]));
    }
}

function LimpaLocalEntregaCTTerceiroConsulta(source, eventArgs) {
    var texto = $('#TxtDescMunicipio_MunicipioEntrega_CteTerceiroConsulta').val();

    if (Trim(texto) == "") {

        $('#TxtCodMunicipio_MunicipioEntrega_CteTerceiroConsulta').val("");
        $('#TxtDescMunicipio_MunicipioEntrega_CteTerceiroConsulta').val("");
        $('#TxtUF_MunicipioEntrega_CteTerceiroConsulta').val("");

    }
}