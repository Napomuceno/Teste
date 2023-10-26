<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConhecimentoOtimizado.aspx.cs" Inherits="Servicelogic.TMS.Web.Carga.Pagina._ConhecimentoOtimizado" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="updateProgress" TagName="UpdateProgress" Src="~/UpdateProgress.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cadastro Otimizado do Conhecimento.::. TMS Web</title>
    <link href="../../../Estilo/Padrao.css" rel="stylesheet" type="text/css" />
    <link href="../../../Estilo/PadraoLista.css" rel="stylesheet" type="text/css" />
    <script src="../../../JavaScript/Padrao.js" type="text/javascript"></script>
    <script src="../../../JavaScript/JQuery.js" type="text/javascript"></script>
    <script src="../../../JavaScript/Mascara.js" type="text/javascript"></script>
    <link href="../../../Estilo/defaultupload.css" rel="stylesheet" type="text/css" />

    <script src="../../../JavaScript/jquery.MultiFile.js" type="text/javascript"></script>
</head>
<body>
    <script>
        document.write('<link href="ConhecimentoOtimizado.css?cache=' + getTicks(new Date()) + '" rel="stylesheet" type="text/css" />')
        document.write('<script src="ConhecimentoOtimizado.js?cache=' + getTicks(new Date()) + '" type="text/javascript">' + '<' + '/script>')
    </script>
    <form id="FrmProgramacao" method="post" runat="server" enableviewstate="true">
        <cc1:ToolkitScriptManager ID="SMProgramacao" runat="server" AsyncPostBackTimeout="9999" EnablePartialRendering="true" EnableScriptGlobalization="true"></cc1:ToolkitScriptManager>
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>--%>
        <div class="ToolBox" enableviewstate="true">
            <div>
                <cc1:ModalPopupExtender ID="ModalProgress" runat="server" TargetControlID="UpdatePanel2" BackgroundCssClass="modalBackground" PopupControlID="panelUpdateProgress" />
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="LblTitulo" runat="server" CssClass="lbltitulo" Text="Cadastro Otimizado"></asp:Label>
                        <asp:Button ID="BtnNovo" runat="server" CssClass="BotaoToolbox" Text="Novo" TabIndex="1" Visible="false" />
                        <asp:Button ID="BtnSalvar" runat="server" CssClass="BotaoToolbox" Text="Salvar" Visible="false" OnClientClick="return ValidarFormularioPreNota();" TabIndex="2" />
                        <asp:Button ID="BtnSalvarNovo" runat="server" CssClass="BotaoToolbox" Text="Salvar e Novo" Visible="false" OnClientClick="return ValidarFormularioPreNota();" />
                        <asp:Button ID="BtnExcluir" runat="server" CssClass="BotaoToolbox" Text="Excluir" Visible="false" OnClientClick="return ConfirmaExclusao();" />
                        <asp:DropDownList ID="Ddlescolha" runat="server" CssClass="BotaoToolbox" Visible="false"></asp:DropDownList>
                        <asp:Button ID="BtnEscolha" runat="server" CssClass="BotaoToolboxOwer" Text="Executar" Visible="false" OnClientClick="return ExecutarEscolhaTela();" />
                        <asp:Button ID="BtnSair" runat="server" CssClass="BotaoToolboxOwer" Text="Sair" Visible="true" OnClick="BtnSair_Click" />
                        <asp:Button ID="BtnOpcoesFiltro" runat="server" CssClass="BotaoToolboxOwer" Enable="false" Text="Opções de Pesquisa" Visible="true" EnableViewState="true" OnClick="BtnAbrirConsultaNotasFiscais_Click" />
                        <asp:Button ID="BtnRetirarInserirNF" runat="server" CssClass="BotaoToolboxOwer" Enable="false" Text="Retirar/Inserir" Visible="true" EnableViewState="true" OnClick="BtnRetirarInserirNF_Click" />
                        <asp:Button ID="BtnCadastrarNovaNota" runat="server" CssClass="BotaoToolboxOwer" Enable="false" Text="Cadastrar Nova Nota" Visible="true" EnableViewState="true" OnClick="BtnAbrirOpcoesConsulta_Click" />                
                       <asp:Button ID="BtnImportarXMLChave" runat="server" CssClass="BotaoToolboxOwer" Enable="false" Text="Importar XML pela chave" ToolTip="Clique aqui para nova importação" EnableViewState="true" OnClick="BtnImportarXMLChave_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnCadastrarNovaNota" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <asp:ImageButton ID="BtnCadastroOtimizado" runat="server" CssClass="BotaoToolboxOwer" ImageUrl="~/Imagem/home.png" Visible="true" EnableViewState="true" OnClick="BtnAbrirCadastroOtimizado_Click" />
            <asp:Button ID="BtnImportarXML" runat="server" CssClass="BotaoToolboxOwer" Enable="false" Text="Importar XML" EnableViewState="true" OnClick="BtnAbrirImportacaoXML_Click" />            
        </div>
        <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
        <%-- MULTIVIEW PRINCIPAL QUE SEPARA A IMPORTACAO DE XML DAS DEMAIS FUNCIONALIDADES --%>
        <asp:MultiView ID="mvwPrincipal" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewPrincipal" runat="server">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="DivLblMensagem">
                            <asp:Label ID="LblMensagem" runat="server" EnableViewState="false" Text=""></asp:Label>
                        </div>
                        <%-- MultiView Geral da pagina --%>
                        <asp:MultiView ID="mvwGeral" ActiveViewIndex="0" runat="server">
                            <%--View onde se gera o conhecimento --%>
                            <asp:View ID="viewGerarConhecimento" runat="server">
                                <div>
                                    <div id="DivCabecalho"></div>
                                    <div id="DivProgramacao"></div>
                                    <div id="DivInforNumeroProgramacao"></div>
                                    <div id="DivSelecionaTipoGeracaoCte">
                                        <asp:Label ID="LblSelecionaTipoGeracaoCte" runat="server" Font-Bold="true" CssClass="lbldescricao" Text="Gerar CT-e por:"></asp:Label>
                                        <asp:RadioButton ID="RbtGeracaoCtePorNota" runat="server" Checked="True" Font-Bold="true" Text="NF-e de Terceiros (Normal)" CssClass="radlist" GroupName="GrpTipoGeracaoCte" onkeypress="return btEnter()" TabIndex="1" onkeydown="return ModifyEnterKeyPressAsTab()" AutoPostBack="true" OnCheckedChanged="RbtGeracaoCtePorNota_Click" />
                                        <asp:RadioButton ID="RbtGeracaoCtePorCte" runat="server" Text="CT-e de Terceiros (SubContratação)" Font-Bold="true" CssClass="radlist" GroupName="GrpTipoGeracaoCte" onkeypress="return btEnter()" TabIndex="2" onkeydown="return ModifyEnterKeyPressAsTab()" AutoPostBack="true" OnCheckedChanged="RbtGeracaoCtePorCte_Click" />
                                    </div>
                                    <div id="DivNotaFiscal">
                                        <asp:Label ID="LblNotasFiscaisDisponiveis" runat="server" CssClass="lbldescricao" Text="Notas Fiscais Disponíveis"></asp:Label>
                                    </div>
                                    <div id="Notas" runat="server" class="DivListaGrid">
                                        <asp:GridView ID="GvwListaNotasDisponiveis" Visible="false" runat="server" AllowSorting="True" Width="100%" CellSpacing="1" CellPadding="0" GridLines="None" AutoGenerateColumns="False" OnSorting="GvwLista_Sorting">
                                            <RowStyle CssClass="primeiroRegistro" />
                                            <HeaderStyle CssClass="headerEstilo" />
                                            <AlternatingRowStyle CssClass="segundoRegistro" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="CifFob" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="CifFob" runat="server" Visible="false" Text='<%# Eval("CifFob") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="RemetenteCNPJCPF" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="RemetenteCNPJCPF" runat="server" Visible="false" Text='<%# Eval("RemetenteCNPJCPF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DestinatarioCnpjCpf" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DestinatarioCnpjCpf" runat="server" Visible="false" Text='<%# Eval("DestinatarioCnpjCpf") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="IdNotaFiscal" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdNotaFiscal" runat="server" Visible="false" Text='<%# Eval("IdNotaFiscal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="selecione" Visible="true" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <div id="divselect" runat="server">
                                                            <asp:CheckBox ID="chkselect" runat="server" onClick="SelecionarCheck(this, 'Normal')" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <input id="chkall" onclick="javascript: selecionatodoschecks(this);" runat="server" type="checkbox" />
                                                    </HeaderTemplate>
                                                    <ItemStyle Width="10px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderImageUrl="~/Imagem/imgAlterar.gif" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnEditar" runat="server" ImageUrl="~/Imagem/imgAlterar.gif" OnCommand="EditarNotaFiscal_Command" CommandArgument='<%# Eval("IdNotaFiscal") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nº NF" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" SortExpression="NumeroNF">
                                                    <ItemTemplate>
                                                        <asp:Label ID="NumeroNF" runat="server" Text='<%# Eval("NumeroNF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Série" SortExpression="SerieNF" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="SerieNF" runat="server" Text='<%# Eval("SerieNF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Emissão" SortExpression="DataEmissao" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DataEmissao" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",Eval("DataEmissao")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Nota" SortExpression="TotalNF" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TotalNF" runat="server" Text='<%# String.Format("{0:C2}",Eval("TotalNF")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remetente" SortExpression="Remetente" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Remetente" runat="server" Text='<%# (Eval("Remetente")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Destinatário" SortExpression="Destinatario" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Destinatario" runat="server" Text='<%# (Eval("Destinatario")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdFilial" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdFilial" runat="server" Visible="false" Text='<%# Eval("IdFilial") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CIF / FOB" SortExpression="CifFob" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="CifFobDesc" runat="server"
                                                            Text='<%# (Eval("CifFob").ToString() == "F"
                                                                        ? "FOB"
                                                                        : "CIF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Filial" SortExpression="Filial" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Filial" runat="server" Text='<%# (Eval("Filial")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdLocalColeta" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdLocalColeta" runat="server" Visible="false" Text='<%# Eval("IdLocalColeta") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdLocalEntrega" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdLocalEntrega" runat="server" Visible="false" Text='<%# Eval("IdLocalEntrega") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Local Coleta" SortExpression="LocalColeta" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Coleta" runat="server" Text='<%#(Eval("LocalColeta")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Local Entrega" SortExpression="LocalEntrega" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Entrega" runat="server" Text='<%# (Eval("LocalEntrega")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FormatoCasasVolume" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="FormatoCasasVolume" runat="server" Visible="false" Text='<%# Eval("FormatoCasasVolume") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Volume" SortExpression="VolumeTotal" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Volume" runat="server" Text='<%# String.Format((Eval("FormatoCasasVolume")).ToString() ,Eval("VolumeTotal")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FormatoCasasPeso" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="FormatoCasasPeso" runat="server" Visible="false" Text='<%# Eval("FormatoCasasPeso") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Peso" SortExpression="PesoTotal" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Peso" runat="server" Text='<%# String.Format((Eval("FormatoCasasPeso")).ToString() ,Eval("PesoTotal")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Peso Cubado" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="PesoCubado" runat="server" Text='<%# String.Format((Eval("FormatoCasasPeso")).ToString(), Eval("PesoCubado")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Natureza" SortExpression="DescNatureza" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DescNatureza" runat="server" Text='<%# Eval("DescNatureza") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Espécie" SortExpression="DescEspecie" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DescEspecie" runat="server" Text='<%# Eval("DescEspecie") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Chave NF-e" SortExpression="ChaveEletronicaNFe" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ChaveEletronicaNFe" runat="server" Text='<%# Eval("ChaveEletronicaNFe") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div id="DivAddNotasSelecionadas">
                                        <asp:LinkButton ID="BtnAddNotasSelecionadas" runat="server" CssClass="lkbSetaDown" onkeydown="return ModifyEnterKeyPressAsTab()" ToolTip="Adicionar à lista de notas selecionadas" OnClick="BtnAdicionar_Click"></asp:LinkButton>
                                    </div>
                                    <asp:LinkButton ID="BtnRemNotasSelecionadas" runat="server" CssClass="lkbSetaUp" onkeydown="return ModifyEnterKeyPressAsTab()" ToolTip="Remover da lista de notas selecionadas" OnClick="BtnRemover_Click"></asp:LinkButton>
                                    <div id="DivNotasFiscaisSelecionadas">
                                        <asp:Label ID="LblNotasFiscaisSelecionadas" runat="server" CssClass="lbldescricao" Text="Notas Fiscais Selecionadas"></asp:Label>
                                    </div>
                                    <div id="DivGridNotasFiscaisSelecionadas" runat="server" class="DivListaGrid">
                                        <asp:GridView ID="GridNotasFiscaisSelecionadas" Visible="false"  runat="server" Width="100%" CellSpacing="1" CellPadding="0" AllowSorting="True" GridLines="None" AutoGenerateColumns="False">
                                            <RowStyle CssClass="primeiroRegistro" />
                                            <HeaderStyle CssClass="headerEstilo" />
                                            <AlternatingRowStyle CssClass="segundoRegistro" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="CifFob" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="CifFob" runat="server" Visible="false" Text='<%# Eval("CifFob") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="RemetenteCNPJCPF" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="RemetenteCNPJCPF" runat="server" Visible="false" Text='<%# Eval("RemetenteCNPJCPF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DestinatarioCnpjCpf" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DestinatarioCnpjCpf" runat="server" Visible="false" Text='<%# Eval("DestinatarioCnpjCpf") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="IdNotaFiscal" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdNotaFiscal" runat="server" Visible="false" Text='<%# Eval("IdNotaFiscal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="selecione" Visible="true" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <div id="divselect" runat="server">
                                                            <asp:CheckBox ID="chkselect" runat="server" onClick="SelecionarCheck(this, 'Normal')" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <input id="chkall" onclick="javascript: selecionatodoschecks(this);" runat="server" type="checkbox" />
                                                    </HeaderTemplate>
                                                    <ItemStyle Width="10px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderImageUrl="~/Imagem/imgAlterar.gif" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnEditar" runat="server" ImageUrl="~/Imagem/imgAlterar.gif" OnCommand="EditarNotaFiscal_Command" CommandArgument='<%# Eval("IdNotaFiscal") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nº NF" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="NumeroNF" runat="server" Text='<%# Eval("NumeroNF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Série" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="SerieNF" runat="server" Text='<%# Eval("SerieNF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Emissão" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DataEmissao" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",Eval("DataEmissao")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Nota" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TotalNF" runat="server" Text='<%# String.Format("{0:C2}",Eval("TotalNF")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remetente" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Remetente" runat="server" Text='<%# (Eval("Remetente")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Destinatário" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Destinatario" runat="server" Text='<%# (Eval("Destinatario")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CIF / FOB" SortExpression="CifFob" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="CifFobDesc" runat="server"
                                                            Text='<%# (Eval("CifFob").ToString() == "F"
                                                                        ? "FOB"
                                                                        : "CIF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdFilial" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdFilial" runat="server" Visible="false" Text='<%# Eval("IdFilial") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Filial" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Filial" runat="server" Text='<%# (Eval("Filial")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdLocalColeta" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdLocalColeta" runat="server" Visible="false" Text='<%# Eval("IdLocalColeta") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdLocalEntrega" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdLocalEntrega" runat="server" Visible="false" Text='<%# Eval("IdLocalEntrega") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Local Coleta" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Coleta" runat="server" Text='<%#(Eval("LocalColeta").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Local Entrega" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Entrega" runat="server" Text='<%# (Eval("LocalEntrega").ToString())%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FormatoCasasVolume" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="FormatoCasasVolume" runat="server" Visible="false" Text='<%# Eval("FormatoCasasVolume") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Volume" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Volume" runat="server" Text='<%# String.Format((Eval("FormatoCasasVolume")).ToString() ,Eval("VolumeTotal")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FormatoCasasPeso" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="FormatoCasasPeso" runat="server" Visible="false" Text='<%# Eval("FormatoCasasPeso") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Peso" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Peso" runat="server" Text='<%# String.Format((Eval("FormatoCasasPeso")).ToString() ,Eval("PesoTotal")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Peso Cubado" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="PesoCubado" runat="server" Text='<%# String.Format((Eval("FormatoCasasPeso")).ToString(), Eval("PesoCubado")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Natureza" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DescNatureza" runat="server" Text='<%# Eval("DescNatureza") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Espécie" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DescEspecie" runat="server" Text='<%# Eval("DescEspecie") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Chave NF-e" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ChaveEletronicaNFe" runat="server" Text='<%# Eval("ChaveEletronicaNFe") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div id="DivCTeTerceiroDisponiveis" runat="server" class="DivListaGrid">
                                        <asp:GridView ID="GvwListaCTeTerceiroDisponiveis" Visible="false" runat="server" AllowSorting="True" Width="100%" CellSpacing="1" CellPadding="0" GridLines="None" AutoGenerateColumns="False" OnSorting="GvwLista_Sorting">
                                            <RowStyle CssClass="primeiroRegistro" />
                                            <HeaderStyle CssClass="headerEstilo" />
                                            <AlternatingRowStyle CssClass="segundoRegistro" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="RemetenteCNPJCPF" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="RemetenteCNPJCPF" runat="server" Visible="false" Text='<%# Eval("RemetenteCNPJCPF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DestinatarioCnpjCpf" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DestinatarioCnpjCpf" runat="server" Visible="false" Text='<%# Eval("DestinatarioCnpjCpf") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="EmitenteCnpjCpf" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="EmitenteCnpjCpf" runat="server" Visible="false" Text='<%# Eval("EmitenteCnpjCpf") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdConhecimentoTerceiro" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdConhecimentoTerceiro" runat="server" Visible="false" Text='<%# Eval("IdConhecimentoTerceiro") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="selecione" Visible="true" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <div id="divselect" runat="server">
                                                            <asp:CheckBox ID="chkselect" runat="server" onClick="SelecionarCheck(this, 'Normal')" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <input id="chkall" onclick="javascript: selecionatodoschecks(this);" runat="server" type="checkbox" />
                                                    </HeaderTemplate>
                                                    <ItemStyle Width="10px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderImageUrl="~/Imagem/imgAlterar.gif" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnEditar" runat="server" ImageUrl="~/Imagem/imgAlterar.gif" OnCommand="EditarConhecimentoTerceiro_Command" CommandArgument='<%# Eval("IdConhecimentoTerceiro") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nº CT-e" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" SortExpression="NumCTRC">
                                                    <ItemTemplate>
                                                        <asp:Label ID="NumCTRC" runat="server" Text='<%# Eval("NumCTRC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Série" SortExpression="SerieCTRC" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="SerieCTRC" runat="server" Text='<%# Eval("SerieCTRC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Emissão" SortExpression="DataEmissao" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DataEmissao" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",Eval("DataEmissao")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Nota" SortExpression="TotalValorNF" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TotalValorNF" runat="server" Text='<%# String.Format("{0:C2}",Eval("TotalValorNF")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Emitente" SortExpression="Emitente" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Emitente" runat="server" Text='<%# (Eval("Emitente")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remetente" SortExpression="Remetente" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Remetente" runat="server" Text='<%# (Eval("Remetente")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Destinatário" SortExpression="Destinatario" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Destinatario" runat="server" Text='<%# (Eval("Destinatario")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdFilial" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdFilial" runat="server" Visible="false" Text='<%# Eval("IdFilial") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Filial" SortExpression="Filial" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Filial" runat="server" Text='<%# (Eval("Filial")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdColeta" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdColeta" runat="server" Visible="false" Text='<%# Eval("IdColeta") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdEntrega" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdEntrega" runat="server" Visible="false" Text='<%# Eval("IdEntrega") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Local Coleta" SortExpression="LocalColeta" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Coleta" runat="server" Text='<%#(Eval("LocalColeta")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Local Entrega" SortExpression="LocalEntrega" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Entrega" runat="server" Text='<%# (Eval("LocalEntrega")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FormatoCasasPeso" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="FormatoCasasPeso" runat="server" Visible="false" Text='<%# Eval("FormatoCasasPeso") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Peso" SortExpression="TotalPesoNF" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TotalPesoNF" runat="server" Text='<%# String.Format((Eval("FormatoCasasPeso")).ToString() ,Eval("TotalPesoNF")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Natureza" SortExpression="DescNatureza" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DescNatureza" runat="server" Text='<%# Eval("DescNatureza") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Espécie" SortExpression="DescEspecie" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DescEspecie" runat="server" Text='<%# Eval("DescEspecie") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Chave CT-e" SortExpression="ChaveCTe" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ChaveCTe" runat="server" Text='<%# Eval("ChaveCTe") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div id="DivCTeTerceiroSelecionadas" runat="server" class="DivListaGrid">
                                        <asp:GridView ID="GvwListaCTeTerceiroSelecionadas" Visible="false" runat="server" AllowSorting="True" Width="100%" CellSpacing="1" CellPadding="0" GridLines="None" AutoGenerateColumns="False" OnSorting="GvwLista_Sorting">
                                            <RowStyle CssClass="primeiroRegistro" />
                                            <HeaderStyle CssClass="headerEstilo" />
                                            <AlternatingRowStyle CssClass="segundoRegistro" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="RemetenteCNPJCPF" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="RemetenteCNPJCPF" runat="server" Visible="false" Text='<%# Eval("RemetenteCNPJCPF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DestinatarioCnpjCpf" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DestinatarioCnpjCpf" runat="server" Visible="false" Text='<%# Eval("DestinatarioCnpjCpf") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="EmitenteCnpjCpf" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="EmitenteCnpjCpf" runat="server" Visible="false" Text='<%# Eval("EmitenteCnpjCpf") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdConhecimentoTerceiro" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdConhecimentoTerceiro" runat="server" Visible="false" Text='<%# Eval("IdConhecimentoTerceiro") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="selecione" Visible="true" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <div id="divselect" runat="server">
                                                            <asp:CheckBox ID="chkselect" runat="server" onClick="SelecionarCheck(this, 'Normal')" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <input id="chkall" onclick="javascript: selecionatodoschecks(this);" runat="server" type="checkbox" />
                                                    </HeaderTemplate>
                                                    <ItemStyle Width="10px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderImageUrl="~/Imagem/imgAlterar.gif" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnEditar" runat="server" ImageUrl="~/Imagem/imgAlterar.gif" OnCommand="EditarConhecimentoTerceiro_Command" CommandArgument='<%# Eval("IdConhecimentoTerceiro") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nº CT-e" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" SortExpression="NumCTRC">
                                                    <ItemTemplate>
                                                        <asp:Label ID="NumCTRC" runat="server" Text='<%# Eval("NumCTRC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Série" SortExpression="SerieCTRC" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="SerieCTRC" runat="server" Text='<%# Eval("SerieCTRC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Emissão" SortExpression="DataEmissao" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DataEmissao" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",Eval("DataEmissao")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Nota" SortExpression="TotalValorNF" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TotalValorNF" runat="server" Text='<%# String.Format("{0:C2}",Eval("TotalValorNF")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Emitente" SortExpression="Emitente" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Emitente" runat="server" Text='<%# (Eval("Emitente")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remetente" SortExpression="Remetente" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Remetente" runat="server" Text='<%# (Eval("Remetente")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Destinatário" SortExpression="Destinatario" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Destinatario" runat="server" Text='<%# (Eval("Destinatario")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdFilial" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdFilial" runat="server" Visible="false" Text='<%# Eval("IdFilial") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Filial" SortExpression="Filial" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Filial" runat="server" Text='<%# (Eval("Filial")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdColeta" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdColeta" runat="server" Visible="false" Text='<%# Eval("IdColeta") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IdEntrega" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="IdEntrega" runat="server" Visible="false" Text='<%# Eval("IdEntrega") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Local Coleta" SortExpression="LocalColeta" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Coleta" runat="server" Text='<%#(Eval("LocalColeta")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Local Entrega" SortExpression="LocalEntrega" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Entrega" runat="server" Text='<%# (Eval("LocalEntrega")).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FormatoCasasPeso" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="FormatoCasasPeso" runat="server" Visible="false" Text='<%# Eval("FormatoCasasPeso") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Peso" SortExpression="TotalPesoNF" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="TotalPesoNF" runat="server" Text='<%# String.Format((Eval("FormatoCasasPeso")).ToString() ,Eval("TotalPesoNF")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Natureza" SortExpression="DescNatureza" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DescNatureza" runat="server" Text='<%# Eval("DescNatureza") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Espécie" SortExpression="DescEspecie" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DescEspecie" runat="server" Text='<%# Eval("DescEspecie") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Chave CT-e" SortExpression="ChaveCTe" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="ChaveCTe" runat="server" Text='<%# Eval("ChaveCTe") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <asp:Panel ID="PnlGerarConhecimento" runat="server" Enabled="false">
                                        <asp:Label ID="LblTipoTransporte" runat="server" Text="Tipo Transporte" CssClass="lbldescricao"></asp:Label>
                                        <asp:DropDownList ID="DdlIdTipoTransporte_TipoTransporte_ConhecimentoOtimizado" runat="server" CssClass="drolistObr" TabIndex="3" onkeydown="return ModifyEnterKeyPressAsTab()" onchange="TratarEventoTabelaFreteGeralM03();"></asp:DropDownList>
                                        <asp:Label ID="lblSerieCTRC" runat="server" Text="Série" CssClass="lbldescricao"></asp:Label>
                                        <asp:TextBox ID="TxtSerieCTRC_ConhecimentoOtimizado" runat="server" CssClass="ctxtObr" TabIndex="4" onkeypress="return btEnter()" onkeydown="return ModifyEnterKeyPressAsTab()" Visible="true"></asp:TextBox>
                                        <asp:Label ID="LblTipoServico" runat="server" Text="Tipo Serviço" CssClass="lbldescricao"></asp:Label>
                                        <asp:DropDownList ID="DdlTipoServicoCte_ConhecimentoOtimizado" runat="server" CssClass="drolistObr" TabIndex="5" onkeydown="return ModifyEnterKeyPressAsTab()" AutoPostBack="False"></asp:DropDownList>

                                        <%--Coleta--%>
                                        <asp:Label ID="LblCodColetaCTRC" runat="server" Text="Local Coleta" EnableViewState="false" ToolTip="Local da Coleta" CssClass="lbldescricao"></asp:Label>
                                        <asp:TextBox ID="TxtCodMunicipio_MunicipioColeta_ConhecimentoOtimizado" Enabled="false" BackColor="WhiteSmoke" runat="server" ToolTip="Pesquise aqui por Local da Coleta" MaxLength="10" CssClass="ctxt"></asp:TextBox>
                                        <asp:TextBox ID="TxtDescMunicipio_MunicipioColeta_ConhecimentoOtimizado" runat="server" ToolTip="Pesquise aqui por Local da Coleta" TabIndex="6" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxtObr" OnkeyUp="return LimpaLocalColeta()"></asp:TextBox>
                                        <asp:TextBox ID="TxtUF_MunicipioColeta_ConhecimentoOtimizado" runat="server" CssClass="ctxt" Enabled="false" BackColor="WhiteSmoke" onkeypress="return btEnter()"></asp:TextBox>
                                        <asp:LinkButton ID="LkbAtualizaColeta" runat="server" ToolTip="Clique aqui para atualizar o local de coleta" CssClass="lkbatualizar" EnableViewState="false" OnClick="LkbAtualizarColeta_Click" TabIndex="7"></asp:LinkButton>
                                        <asp:LinkButton ID="LkbEditarColeta" runat="server" CssClass="lkbeditar" ToolTip="Clique aqui para editar ou cadastrar um novo local de coleta" EnableViewState="false" OnClick="LkbEditaLocalColeta_Click" TabIndex="7"></asp:LinkButton>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" TargetControlID="TxtDescMunicipio_MunicipioColeta_ConhecimentoOtimizado" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaMunicipioCodigoNome" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDMunicipioLocalColeta" ContextKey="Municipio,DescMunicipio" MinimumPrefixLength="1" />
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender15" runat="server" TargetControlID="TxtDescMunicipio_MunicipioColeta_ConhecimentoOtimizado" WatermarkCssClass="ctxtMDObr" WatermarkText="Pesquise aqui por Código ou Descrição..." />
                                        <%--CifFob--%>
                                        <div id="FobCif">
                                            <asp:RadioButton ID="Rb1CifFob_ConhecimentoOtimizado" runat="server" Checked="True" Text="1 - Pago" ToolTip="CIF" CssClass="radlist" ReadOnly="true" GroupName="GrpCifFob" onkeypress="return btEnter()" TabIndex="8" onkeydown="return ModifyEnterKeyPressAsTab()" AutoPostBack="true" OnCheckedChanged="CifFob_Click" />
                                            <asp:RadioButton ID="Rb2CifFob_ConhecimentoOtimizado" runat="server" Text="2 - A Pagar" ToolTip="FOB" CssClass="radlist" ReadOnly="true" GroupName="GrpCifFob" onkeypress="return btEnter()" TabIndex="9" onkeydown="return ModifyEnterKeyPressAsTab()" AutoPostBack="true" OnCheckedChanged="CifFob_Click" />
                                            <asp:RadioButton ID="Rb3CifFob_ConhecimentoOtimizado" runat="server" Text="3 - Outros" ToolTip="Outros" CssClass="radlist" ReadOnly="true" GroupName="GrpCifFob" onkeypress="return btEnter()" TabIndex="10" onkeydown="return ModifyEnterKeyPressAsTab()" AutoPostBack="true" OnCheckedChanged="CifFob_Click" />
                                        </div>
                                        <%--Pagador--%>
                                        <asp:Label ID="LblCodPagador" runat="server" Text="Tomador Serviço" CssClass="lbldescricao" EnableViewState="false"></asp:Label>
                                        <asp:TextBox ID="TxtCodCliente_Pagador_ConhecimentoOtimizado" runat="server" MaxLength="10" CssClass="ctxt" BackColor="WhiteSmoke" Enabled="false" onkeypress="return btEnter()" ToolTip="Código do Tomador Serviço. Pesquise pelo nome no campo ao lado caso não saiba o código"></asp:TextBox>
                                        <asp:TextBox ID="TxtNome_Pagador_ConhecimentoOtimizado" runat="server" CssClass="ctxtObr" onkeypress="return btEnter()" TabIndex="11" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaPagador()"></asp:TextBox>
                                        <asp:TextBox ID="TxtCnpjCpf_Pagador_ConhecimentoOtimizado" Visible="true" Enabled="false" runat="server" CssClass="ctxt" onkeypress="return btEnter()" BackColor="WhiteSmoke"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" TargetControlID="TxtNome_Pagador_ConhecimentoOtimizado" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodigoCnpjCpfPessoa" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDCodigoCnpjCpfPagador" ContextKey="Cliente,CnpjCpf" MinimumPrefixLength="1" />
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender12" runat="server" TargetControlID="TxtNome_Pagador_ConhecimentoOtimizado" WatermarkCssClass="ctxtMDObr" WatermarkText="Pesquise aqui por Código, Nome, CNPJ ou CPF..." />
                                        <asp:LinkButton ID="LkbAtualizarPagador" runat="server" CssClass="lkbatualizar" ToolTip="Clique aqui para atualizar o Tomador Serviço" EnableViewState="false" OnClick="LkbAtualizarPagador_Click" TabIndex="12"></asp:LinkButton>
                                        <asp:LinkButton ID="LkbEditarPagador" runat="server" CssClass="lkbeditar" EnableViewState="false" ToolTip="Clique aqui para editar o Tomador Serviço" OnClick="LkbEditarPagador_Click" TabIndex="13"></asp:LinkButton>
                                        <%--Veículo--%>
                                        <asp:Label ID="LblVeiculo" runat="server" Text="Veículo Tração" CssClass="lbldescricao"></asp:Label>
                                        <asp:TextBox ID="TxtCodVeiculo_Veiculo_ConhecimentoOtimizado" runat="server" Enabled="false" BackColor="WhiteSmoke" MaxLength="10" CssClass="ctxt"></asp:TextBox>
                                        <asp:TextBox ID="TxtPlaca_Veiculo_ConhecimentoOtimizado" runat="server" OnkeyUp="return LimpaVeiculo()" MaxLength="10" CssClass="ctxtObr" TabIndex="14" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" TargetControlID="TxtPlaca_Veiculo_ConhecimentoOtimizado" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaVeiculoTracaoAtivos" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDVeiculo" ContextKey="Veiculo,Placa" MinimumPrefixLength="1" />
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="TxtPlaca_Veiculo_ConhecimentoOtimizado" WatermarkCssClass="ctxtMDObr" WatermarkText="Código ou Placa..." />
                                        <asp:LinkButton ID="LkbAtualizarVeiculo" runat="server" ToolTip="Clique aqui para atualizar o veículo" CssClass="lkbatualizar" OnClick="LkbAtualizarVeiculo_Click" TabIndex="15"></asp:LinkButton>
                                        <asp:LinkButton ID="LkbEditarVeiculo" runat="server" CssClass="lkbeditar" ToolTip="Clique aqui para editar ou cadastrar um novo veículo" EnableViewState="false" OnClick="LkbEditarVeiculo_Click" TabIndex="16"></asp:LinkButton>
                                        <asp:Panel ID="PnlProgramacao" runat="server" Visible="false">
                                            <asp:Label ID="LblProgramacao" runat="server" Text="Programação" CssClass="lbldescricao"></asp:Label>
                                            <asp:TextBox ID="TxtNumProgramacao_ConhecimentoOtimizado" runat="server" alt="integerde10" CssClass="ctxt"></asp:TextBox>
                                        </asp:Panel>
                                        <div>
                                            <asp:LinkButton ID="LkbTabelaFrete" runat="server" ToolTip="Clique aqui para selecionar a Tabela de Frete !"
                                                Text="Tabela de Frete" OnClientClick="ListarTabelaFrete();">Tabela de Frete</asp:LinkButton>
                                        </div>
                                        <div id="DivGerarCTRB">
                                            <asp:CheckBox ID="ckbGerarCTRB_ConhecimentoOtimizado" runat="server" CssClass="chkb" Text=" Gerar Contrato de Transporte" EnableViewState="true" TabIndex="17" Checked="false"></asp:CheckBox>
                                        </div>
                                        <div id="DivUtilizaTabelaFrete">
                                        </div>
                                        <div id="DivChkConsiderarCubagemMercadoriaPadraoTabelaFreteFracionadaModelo05">
                                            <asp:CheckBox ID="ChkConsiderarCubagemMercadoriaPadraoTabelaFreteFracionadaModelo05" runat="server" CssClass="chkb" Text="Considerar a cubagem de mercadoria padrão para a tabela de frete fracionada modelo 05" TabIndex="17" Checked="false" OnCheckedChanged="ChkConsiderarCubagemMercadoriaPadraoTabelaFreteFracionadaModelo05_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                        </div>
                                        <asp:Label ID="LblPesoCubadoTotal" runat="server" CssClass="lbldescricao" Text="Peso (M³)"></asp:Label>
                                        <asp:ImageButton ID="BtnComporPesoCubadoTotal" runat="server" ImageUrl="~/Imagem/imgLupa.png" OnClick="BtnComporPesoCubadoTotal_Click" TabIndex="18"
                                            ToolTip="Clique aqui para calcular o Peso Cubado em função das medidas do frete e um fator de cubagem." />
                                        <asp:HiddenField ID="HidVolumeTotal" runat="server" />
                                        <asp:TextBox ID="TxPTotalPesoCubadoNF_ConhecimentoOtimizado" runat="server" CssClass="ctxt" TabIndex="18"></asp:TextBox>
                                        <asp:Panel ID="PnlAcoesViagem" runat="server" Visible="false">
                                            <asp:Label ID="LblAcoes" runat="server" Text="Ação:" CssClass="lbldescricao"></asp:Label>
                                            <div id="DivAcoes">
                                                <asp:RadioButton ID="Rb1GerarViagem" runat="server" onclick="HiddenCamporRelacionarViagem();" Text="Gerar Viagem" CssClass="radlist" GroupName="GrpAcoes" onkeypress="return btEnter()" TabIndex="18" onkeydown="return ModifyEnterKeyPressAsTab()" />
                                                <asp:RadioButton ID="Rb2RelacionarViagem" runat="server" onclick="ExibeCamporRelacionarViagem();" Text="Relacionar Viagem" CssClass="radlist" GroupName="GrpAcoes" onkeypress="return btEnter()" TabIndex="19" onkeydown="return ModifyEnterKeyPressAsTab()" />
                                                <asp:RadioButton ID="Rb2Nenhum" runat="server" onclick="HiddenCamporRelacionarViagem();" Checked="True" Text="Nenhum" CssClass="radlist" GroupName="GrpAcoes" onkeypress="return btEnter()" TabIndex="20" onkeydown="return ModifyEnterKeyPressAsTab()" />
                                            </div>
                                            <asp:Label ID="LblRelacionarViagem" runat="server" Text="Num. Viagem:" CssClass="lbldescricao"></asp:Label>
                                            <asp:TextBox ID="TxtNumViagem_ConhecimentoOtimizado" runat="server" MaxLength="10" alt="integerde8" CssClass="ctxtObr" TabIndex="20" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:TextBox>
                                        </asp:Panel>
                                        <div id="DivBtnGerarConhecimento">
                                            <asp:Button ID="BtnAddComponente" runat="server" CssClass="BotaoToolboxOwer" Text="Gerar Conhecimento" OnClick="GerarCTRC" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="PnlRotaCTe" runat="server" Enabled="false">
                                        <asp:Label ID="LblRotaCTe" runat="server" Text="Rota" CssClass="lbldescricao"></asp:Label>
                                        <asp:DropDownList ID="DdlIdRotaCTe_RotaCTe_ConhecimentoOtimizado" runat="server" CssClass="drolist" TabIndex="18" AutoPostBack="true" OnSelectedIndexChanged="sugerirTomadorRotaCTeCadastroOtimizado" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:DropDownList>
                                        <asp:LinkButton ID="LkbAtualizarRotaCTe" runat="server" ToolTip="Clique aqui para atualizar o veículo" CssClass="lkbatualizar" OnClick="LkbAtualizarRotaCTe_Click" TabIndex="19"></asp:LinkButton>
                                        <asp:LinkButton ID="LkbEditarRotaCTe" runat="server" CssClass="lkbeditar" ToolTip="Clique aqui para editar ou cadastrar um novo veículo" EnableViewState="false" OnClick="LkbEditarRotaCTe_Click" TabIndex="20"></asp:LinkButton>
                                    </asp:Panel>
                                    <input type="hidden" runat="server" id="HIDPossuiFreteCargaGeralM03" value="0" enableviewstate="true" />
                                    <asp:Button ID="BtnTratarTipoOperacao" runat="server" CssClass="BotaoToolboxProximo"
                                        OnClick="BtnTratarTipoOperacao_Click" Style="display: none" />
                                    <asp:Panel ID="pnlTipoOperacaoTransporte" runat="server" Enabled="false">
                                        <div id="DivTipoOperacao">
                                            <asp:Label ID="LblTipoOperacaoTransporte" runat="server" Text="Operação" CssClass="lbldescricao"></asp:Label>
                                            <asp:DropDownList ID="DdlTipoOperacaoTransporte_ConhecimentoOtimizado" onchange="AtivaDesativaTotalPellets(this);" AutoPostBack="true" OnSelectedIndexChanged="BtnCalcularTotalPallets_Click" runat="server" CssClass="drolist" TabIndex="20" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:DropDownList>
                                        </div>
                                        <div id="DivTotalPallets">
                                            <asp:Label ID="LblTotalPallets" runat="server" Text="Total Pallets" CssClass="lbldescricao"></asp:Label>
                                            <asp:TextBox ID="TxtTotalPallets_ConhecimentoOtimizado" Text="" Enabled="false" alt="integerde10" runat="server" TabIndex="22" CssClass="ctxt" onkeypress="return btEnter()" />
                                        </div>
                                    </asp:Panel>
                                </div>
                            </asp:View>
                            <%-- View onde se cadastra novas notas--%>
                            <asp:View ID="viewCadastroNotas" runat="server">
                                <div id="DivCadastroNotas">
                                    <asp:Label ID="LblCadastroNotas" runat="server" CssClass="lbldescricao" Text="Cadastro de Notas Fiscais"></asp:Label>
                                </div>
                                <div id="DivPnlCadastroNotas"></div>
                                <asp:HiddenField ID="HdfIdNotaFiscal_NotaFiscal" runat="server" />
                                <asp:Label ID="LblFilial" runat="server" Text="Filial" CssClass="lbldescricao"></asp:Label>
                                <asp:DropDownList ID="DdlIdFilial_Filial_NotaFiscal" runat="server" CssClass="drolistObr" TabIndex="1" onkeydown="return ModifyEnterKeyPressAsTab()" OnSelectedIndexChanged="FormatarPesoVolume" AutoPostBack="true"></asp:DropDownList>
                                <asp:LinkButton ID="LkbAtualizarFilial" runat="server" CssClass="lkbatualizar" ToolTip="Clique aqui para atualizar o filial" EnableViewState="false" OnClick="LkbAtualizarFilial_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LkbEditarFilial" runat="server" CssClass="lkbeditar" ToolTip="Clique aqui para editar ou cadastrar um novo filial" EnableViewState="false" OnClick="LkbEditarFilial_Click"></asp:LinkButton>
                                <asp:Label ID="LblCodRemetente" runat="server" Text="Remetente" CssClass="lbldescricao" EnableViewState="false"></asp:Label>
                                <asp:TextBox ID="TxtCodCliente_Remetente_NotaFiscal" runat="server" MaxLength="10" CssClass="ctxt" BackColor="WhiteSmoke" Enabled="false" onkeypress="return btEnter()" ToolTip="Código do Remetente. Pesquise pelo nome no campo ao lado caso não saiba o código"></asp:TextBox>
                                <asp:TextBox ID="TxtNome_Remetente_NotaFiscal" runat="server" CssClass="ctxtObr" onkeypress="return btEnter()" TabIndex="30" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaRemetente()"></asp:TextBox>
                                <asp:TextBox ID="TxtCnpjCpf_Remetente_NotaFiscal" Visible="true" Enabled="false" runat="server" CssClass="ctxt" onkeypress="return btEnter()" BackColor="WhiteSmoke"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" TargetControlID="TxtNome_Remetente_NotaFiscal" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodigoCnpjCpfPessoa" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDCodigoCnpjCpfCliente" ContextKey="Cliente,CnpjCpf" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender7" runat="server" TargetControlID="TxtNome_Remetente_NotaFiscal" WatermarkCssClass="ctxtMDObr" WatermarkText="Pesquise aqui por Código, Nome, CNPJ ou CPF..." />
                                <asp:LinkButton ID="LkbAtualizarRemetente" runat="server" CssClass="lkbatualizar" ToolTip="Clique aqui para atualizar o remetente" EnableViewState="false" OnClick="LkbAtualizarRemetente_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LkbEditarRemetente" runat="server" CssClass="lkbeditar" EnableViewState="false" ToolTip="Clique aqui para editar o remetente" OnClick="LkbEditarRemetente_Click"></asp:LinkButton>
                                <asp:Label ID="LblCodDestinatario" runat="server" Text="Destinatário" CssClass="lbldescricao" EnableViewState="false"></asp:Label>
                                <asp:TextBox ID="TxtCodCliente_Destinatario_NotaFiscal" runat="server" MaxLength="10" CssClass="ctxt" Enabled="false" BackColor="WhiteSmoke" onkeypress="return btEnter()" ToolTip="Código do Destinatário. Pesquise pelo nome no campo ao lado caso não saiba o código"></asp:TextBox>
                                <asp:TextBox ID="TxtNome_Destinatario_NotaFiscal" runat="server" CssClass="ctxtObr" onkeypress="return btEnter()" TabIndex="40" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaDestinatario()"></asp:TextBox>
                                <asp:TextBox ID="TxtCnpjCpf_Destinatario_NotaFiscal" Visible="true" Enabled="false" runat="server" CssClass="ctxt" onkeypress="return btEnter()" BackColor="WhiteSmoke"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" TargetControlID="TxtNome_Destinatario_NotaFiscal" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodigoCnpjCpfPessoa" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDCodigoCnpjCpfDestinatario" ContextKey="Cliente,CnpjCpf" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="TxtNome_Destinatario_NotaFiscal" WatermarkCssClass="ctxtMDObr" WatermarkText="Pesquise aqui por Código, Nome, CNPJ ou CPF..." />
                                <asp:LinkButton ID="LkbAtualizarDestinatario" runat="server" CssClass="lkbatualizar" ToolTip="Clique aqui para atualizar o destinatário" EnableViewState="false" OnClick="LkbAtualizarDestinatario_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LkbEditarDestinatario" runat="server" CssClass="lkbeditar" EnableViewState="false" ToolTip="Clique aqui para editar o destinatário" OnClick="LkbEditarDestinatario_Click"></asp:LinkButton>
                                <asp:Label ID="LblTipoNota" runat="server" Text="Tipo de Nota" CssClass="lbldescricao"></asp:Label>
                                <div id="rdTipoNota">
                                    <asp:RadioButton ID="Rb1TipoNotaFiscal_NotaFiscal" runat="server" TabIndex="800" onkeydown="return ModifyEnterKeyPressAsTab()" Checked="True" Text="1 - Venda" CssClass="radlist" GroupName="GrpTipoNota" />
                                    <asp:RadioButton ID="Rb2TipoNotaFiscal_NotaFiscal" runat="server" Text="2 - Transferência" TabIndex="810" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="radlist" GroupName="GrpTipoNota" />
                                </div>
                                <asp:Label ID="LblNF" runat="server" Text="Nº Nota Fiscal" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtNumeroNF_NotaFiscal" runat="server" MaxLength="10" TabIndex="820" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxtCodObr" alt="integerde10"></asp:TextBox>
                                <asp:LinkButton ID="LkbAtualizarNota" runat="server" CssClass="lkbatualizar" ToolTip="Clique aqui para atualizar" EnableViewState="false"></asp:LinkButton>
                                <asp:LinkButton ID="LkbEditarNota" runat="server" CssClass="lkbeditar" EnableViewState="false" ToolTip="Clique aqui para editar o Nota" OnClick="LkbEditarNota_Click"></asp:LinkButton>
                                <asp:Label ID="LblSerie" runat="server" Text="Série" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtSerieNF_NotaFiscal" runat="server" MaxLength="3" TabIndex="830" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxtCodObr"></asp:TextBox>
                                <asp:Label ID="LblTotalNF" runat="server" Text="Total NF" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtTotalNF_NotaFiscal" runat="server" MaxLength="15" TabIndex="840" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxtObr" alt="decimal15de2"></asp:TextBox>
                                <asp:Label ID="LblDataEmissao" runat="server" Text="Data Emissão" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtDataEmissao_NotaFiscal" runat="server" MaxLength="10" CssClass="ctxtObr" TabIndex="850" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="TxtDataEmissao_NotaFiscal" MaskType="Date" Mask="99/99/9999" runat="server" />
                                <input id="BtnCalendarioDataEmissao" type="button" value=" " visible="true" class="BotaoCalendario" tabindex="851" />
                                <cc1:CalendarExtender ID="CalendarExtender2" TargetControlID="TxtDataEmissao_NotaFiscal" PopupButtonID="BtnCalendarioDataEmissao" PopupPosition="BottomLeft" CssClass="Calendario" runat="server" />
                                <asp:Label ID="LblPrevisaoEntrega" runat="server" Text="Previsão Entrega" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtPrevisaoEntrega_NotaFiscal" runat="server" MaxLength="10" CssClass="ctxt" TabIndex="852" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="TxtPrevisaoEntrega_NotaFiscal" MaskType="Date" Mask="99/99/9999" runat="server" />
                                <input id="BtnCalendarioPrevisaoEntrega" type="button" value=" " visible="true" class="BotaoCalendario" tabindex="853" />
                                <cc1:CalendarExtender ID="CalendarExtender1" TargetControlID="TxtPrevisaoEntrega_NotaFiscal" PopupButtonID="BtnCalendarioPrevisaoEntrega" PopupPosition="BottomLeft" CssClass="Calendario" runat="server" />
                                <asp:Label ID="LblModeloNF" runat="server" Text="Modelo NF" CssClass="lbldescricao"></asp:Label>
                                <asp:DropDownList ID="DdlIdModeloNF_NotaFiscal" runat="server" CssClass="drolistObr" TabIndex="860" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:DropDownList>
                                <asp:LinkButton ID="LkbAtualizarModeloNF" runat="server" ToolTip="Clique aqui para atualizar o modelo NF" CssClass="lkbatualizar" OnClick="LkbAtualizarModeloNF_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LkbEditarModeloNF" runat="server" CssClass="lkbeditar" EnableViewState="false" ToolTip="Clique aqui para editar ou cadastrar um novo modelo NF" OnClick="LkbEditaModeloNF_Click"></asp:LinkButton>
                                <asp:Label ID="LblVolume" runat="server" Text="Volume" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxVVolumeTotal_NotaFiscal" runat="server" MaxLength="15" TabIndex="870" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxtObr"></asp:TextBox>
                                <asp:Label ID="LblPeso" runat="server" Text="Peso (Kg)" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxPPesoTotal_NotaFiscal" runat="server" MaxLength="12" TabIndex="880" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxtObr"></asp:TextBox>
                                <asp:Label ID="LblPesoCubado_NotaFiscal" runat="server" Text="Peso Cub. (Kg)" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxPPesoCubado_NotaFiscal" runat="server" MaxLength="12" TabIndex="885" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt"></asp:TextBox>
                                <asp:ImageButton ID="BtnComporPesoCubado" runat="server" ImageUrl="~/Imagem/imgLupa.png" OnClick="LkbComporPesoCubado_Click" OnClientClick="return CalculaVolumeComposicaoNota()" TabIndex="885" />
                                <asp:Label ID="LblNatureza" runat="server" Text="Natureza" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtCodNatureza_Natureza_NotaFiscal" runat="server" Enabled="false" BackColor="WhiteSmoke" CssClass="ctxt"></asp:TextBox>
                                <asp:TextBox ID="TxtDescNatureza_Natureza_NotaFiscal" runat="server" CssClass="ctxtObr" TabIndex="890" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaNatureza()"></asp:TextBox>
                                <asp:LinkButton ID="LkbAtualizarNatureza" runat="server" CssClass="lkbatualizar" ToolTip="Clique aqui para atualizar a natureza" EnableViewState="false" OnClick="LkbAtualizarNatureza_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LkbEditarNatureza" runat="server" CssClass="lkbeditar" ToolTip="Clique aqui para editar a natureza" EnableViewState="false" OnClick="LkbEditarNatureza_Click"></asp:LinkButton>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtenderNatureza" runat="server" TargetControlID="TxtDescNatureza_Natureza_NotaFiscal" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodDescr" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDNaturezaCodDesc" ContextKey="Natureza,DescNatureza" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderNatureza" runat="server" TargetControlID="TxtDescNatureza_Natureza_NotaFiscal" WatermarkCssClass="ctxtMDObr" WatermarkText="Pesquise aqui por Codigo ou Descrição da Natureza..." />
                                <asp:Label ID="LblEspecie" runat="server" Text="Espécie" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtCodEspecie_Especie_NotaFiscal" runat="server" Enabled="false" BackColor="WhiteSmoke" CssClass="ctxt" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:TextBox>
                                <asp:TextBox ID="TxtDescEspecie_Especie_NotaFiscal" runat="server" CssClass="ctxtObr" TabIndex="900" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaEspecie()"></asp:TextBox>
                                <asp:LinkButton ID="LkbAtualizarEspecie" runat="server" CssClass="lkbatualizar" EnableViewState="false" OnClick="LkbAtualizarEspecie_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LkbEditarEspecie" runat="server" CssClass="lkbeditar" EnableViewState="false" OnClick="LkbEditarEspecie_Click"></asp:LinkButton>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtenderEspecie" runat="server" TargetControlID="TxtDescEspecie_Especie_NotaFiscal" CompletionInterval="8" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodDescr" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDEspecieCodDescr" ContextKey="Especie,DescEspecie" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderEspecie" runat="server" TargetControlID="TxtDescEspecie_Especie_NotaFiscal" WatermarkCssClass="ctxtMDObr" WatermarkText="Pesquise aqui por Codigo ou Descrição da Espécie..." />
                                <%--Coleta--%>
                                <asp:Label ID="LblLocalColeta" runat="server" Text="Local Coleta" EnableViewState="false" ToolTip="Local da Coleta" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtCodMunicipio_MunicipioColeta_NotaFiscal" Enabled="false" BackColor="WhiteSmoke" runat="server" ToolTip="Pesquise aqui por Local da Coleta" MaxLength="10" CssClass="ctxt"></asp:TextBox>
                                <asp:TextBox ID="TxtDescMunicipio_MunicipioColeta_NotaFiscal" runat="server" ToolTip="Pesquise aqui por Local da Coleta" TabIndex="910" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxtObr" OnkeyUp="return LimpaLocalColetaNF()"></asp:TextBox>
                                <asp:TextBox ID="TxtUF_MunicipioColeta_NotaFiscal" runat="server" CssClass="ctxt" Enabled="false" BackColor="WhiteSmoke" onkeypress="return btEnter()"></asp:TextBox>
                                <asp:LinkButton ID="LkbAtualizarLocalColeta" runat="server" ToolTip="Clique aqui para atualizar o local de coleta" CssClass="lkbatualizar" EnableViewState="false" OnClick="LkbAtualizarLocalColeta_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LkbEditarLocalColeta" runat="server" CssClass="lkbeditar" ToolTip="Clique aqui para editar ou cadastrar um novo local de coleta" EnableViewState="false" OnClick="LkbEditaLocalColetaNF_Click"></asp:LinkButton>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtenderLocalColeta" runat="server" TargetControlID="TxtDescMunicipio_MunicipioColeta_NotaFiscal" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaMunicipioCodigoNome" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDMunicipioColeta" ContextKey="Municipio,DescMunicipio" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderLocalColeta" runat="server" TargetControlID="TxtDescMunicipio_MunicipioColeta_NotaFiscal" WatermarkCssClass="ctxtMDObr" WatermarkText="Pesquise aqui por Código ou Local de Coleta..." />
                                <%--Coleta--%>
                                <%--Entrega--%>
                                <asp:Label ID="LblLocalEntrega" runat="server" Text="Local Entrega" EnableViewState="false" ToolTip="Local da Entrega" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtCodMunicipio_MunicipioEntrega_NotaFiscal" Enabled="false" BackColor="WhiteSmoke" runat="server" ToolTip="Pesquise aqui por Local da Entrega" MaxLength="10" CssClass="ctxt"></asp:TextBox>
                                <asp:TextBox ID="TxtDescMunicipio_MunicipioEntrega_NotaFiscal" runat="server" ToolTip="Pesquise aqui por Local da Coleta" TabIndex="920" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxtObr" OnkeyUp="return LimpaLocalEntregaNF()"></asp:TextBox>
                                <asp:TextBox ID="TxtUF_MunicipioEntrega_NotaFiscal" runat="server" CssClass="ctxt" Enabled="false" BackColor="WhiteSmoke" onkeypress="return btEnter()"></asp:TextBox>
                                <asp:LinkButton ID="LkbAtualizarEntrega" runat="server" ToolTip="Clique aqui para atualizar o local de entrega" CssClass="lkbatualizar" EnableViewState="false" OnClick="LkbAtualizarEntrega_Click"></asp:LinkButton>
                                <asp:LinkButton ID="LkbEditarEntrega" runat="server" CssClass="lkbeditar" ToolTip="Clique aqui para editar ou cadastrar um novo local de entrega" EnableViewState="false" OnClick="LkbEditaEntrega_Click"></asp:LinkButton>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="TxtDescMunicipio_MunicipioEntrega_NotaFiscal" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaMunicipioCodigoNome" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDMunicipioEntrega" ContextKey="Municipio,DescMunicipio" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="TxtDescMunicipio_MunicipioEntrega_NotaFiscal" WatermarkCssClass="ctxtMDObr" WatermarkText="Pesquise aqui por  Código ou Local da Entrega..." />
                                <%--Entrega--%>
                                <asp:TextBox ID="TxtCodCFOP_CFOP_NotaFiscal" runat="server" CssClass="ctxt" Enabled="false" BackColor="WhiteSmoke"></asp:TextBox>
                                <asp:TextBox ID="TxtDescCFOP_CFOP_NotaFiscal" runat="server" CssClass="ctxtObr" TabIndex="923" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimparCFOP()"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" TargetControlID="TxtDescCFOP_CFOP_NotaFiscal" CompletionInterval="17" Enabled="true" CompletionListCssClass="ConsultaMin" CompletionSetCount="20" ServiceMethod="ConsultaCodDescr" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDCFOPCodDescr" ContextKey="CFOP,DescCfop" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" TargetControlID="TxtDescCFOP_CFOP_NotaFiscal" WatermarkCssClass="ctxtMDObr" WatermarkText="Pesquise aqui por Código ou Descrição..." />
                                <asp:Label ID="LblNumPedido" runat="server" Text="Nº Pedido" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtNumeroPedido_NotaFiscal" runat="server" TabIndex="925" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt" MaxLength="20"></asp:TextBox>
                                <asp:Label ID="LblNCargaNF" runat="server" Text="Nº Carga" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtNumeroCarga_NotaFiscal" runat="server" TabIndex="930" onkeydown="return ModifyEnterKeyPressAsTab()" MaxLength="15" CssClass="ctxt"></asp:TextBox>
                                <asp:Label ID="LblCfopNota" runat="server" Text="CFOP" CssClass="lbldescricao"></asp:Label>
                                <asp:Label ID="LblChaveNFe" runat="server" Text="Chave NF-e" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtChaveEletronicaNFe_NotaFiscal" runat="server" TabIndex="950" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt" MaxLength="44"></asp:TextBox>
                                <div id="DivChkInutilizada">
                                    <asp:CheckBox ID="chkInutilizada_NotaFiscal" runat="server" CssClass="chkb" Text="Indisponível para utilização em CT-e e Cadastro Otimizado" TabIndex="46" />
                                </div>
                                <asp:Button ID="BtnSalvarNotaFiscal" runat="server" CssClass="BotaoToolbox" TabIndex="960" Text="Salvar Nota" ToolTip="Clique aqui para salvar a nota" OnClick="BtnSalvarNotaFiscal_Click" />
                                <asp:Button ID="BtnSalvarNovoNotaFiscal" runat="server" CssClass="BotaoToolbox" Text="Salvar/Novo" ToolTip="Clique aqui para salva/novo" OnClick="BtnSalvarNovoNotaFiscal_Click" />
                                <asp:Button ID="BtnAbortarNotaFiscal" runat="server" CssClass="BotaoToolbox" TabIndex="970" Text="Cancelar" ToolTip="Clique aqui para cancela e volta a lista das notas" OnClick="BtnAbortarNotaFiscal_Click" />
                            </asp:View>
                            <%--View onde se realiza consultas das notas--%>
                            <asp:View ID="viewConsultaNotas" runat="server">
                                <div id="DivConsultarNotas">
                                    <asp:Label ID="LblConsultarNotas" runat="server" CssClass="lbldescricao" Text="Informe Opções de Filtro de Notas Fiscais"></asp:Label>
                                </div>
                                <div id="DivCabecalhoModal"></div>
                                <asp:Label ID="LblTipoConsulta" runat="server" Text="Consultar" CssClass="lbldescricao"></asp:Label>
                                <asp:DropDownList ID="DdlTipoConsulta" runat="server" CssClass="drolistObr" onkeydown="return ModifyEnterKeyPressAsTab()" TabIndex="10"></asp:DropDownList>
                                <asp:Label ID="LblFilialConsulta" runat="server" Text="Filial" CssClass="lbldescricao"></asp:Label>
                                <asp:DropDownList ID="DdlIdFilial_Filial_NotaFiscalConsulta" runat="server" CssClass="drolist" TabIndex="10" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:DropDownList>
                                <asp:Label ID="LblNFConsulta" runat="server" Text="Nº Nota Fiscal" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtNumeroNF_NotaFiscalConsulta" runat="server" MaxLength="10" TabIndex="20" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt" alt="integerde10"></asp:TextBox>
                                <asp:Label ID="LblDataEmissaoConsultaInicial" runat="server" Text="Emissão" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtDataEmissaoConsultaInicial" runat="server" MaxLength="10" CssClass="ctxt" TabIndex="30" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtenderConsultaInicial" TargetControlID="TxtDataEmissaoConsultaInicial" MaskType="Date" Mask="99/99/9999" runat="server" />
                                <input id="BtnCalendarioConsultaDataEmissaoInicial" type="button" value=" " visible="true" class="BotaoCalendario" />
                                <cc1:CalendarExtender ID="CalendarExtender3" TargetControlID="TxtDataEmissaoConsultaInicial" PopupButtonID="BtnCalendarioConsultaDataEmissaoInicial" PopupPosition="BottomLeft" CssClass="Calendario" runat="server" />
                                <asp:Label ID="LblCodRemetenteConsulta" runat="server" Text="Remetente" CssClass="lbldescricao" EnableViewState="false"></asp:Label>
                                <asp:TextBox ID="TxtCodCliente_Remetente_NotaFiscalConsulta" runat="server" MaxLength="10" CssClass="ctxt" BackColor="WhiteSmoke" Enabled="false" onkeypress="return btEnter()" ToolTip="Código do Remetente. Pesquise pelo nome no campo ao lado caso não saiba o código"></asp:TextBox>
                                <asp:TextBox ID="TxtNome_Remetente_NotaFiscalConsulta" runat="server" CssClass="ctxt" onkeypress="return btEnter()" TabIndex="50" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaRemetenteConsulta()"></asp:TextBox>
                                <asp:TextBox ID="TxtCnpjCpf_Remetente_NotaFiscalConsulta" Visible="true" Enabled="false" runat="server" CssClass="ctxt" onkeypress="return btEnter()" BackColor="WhiteSmoke"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="TxtNome_Remetente_NotaFiscalConsulta" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodigoCnpjCpfPessoa" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDCodigoCnpjCpfClienteConsulta" ContextKey="Cliente,CnpjCpf" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="TxtNome_Remetente_NotaFiscalConsulta" WatermarkCssClass="ctxtMD" WatermarkText="Pesquise aqui por Código, Nome, CNPJ ou CPF..." />
                                <asp:Label ID="LblCodDestinatarioConsulta" runat="server" Text="Destinatário" CssClass="lbldescricao" EnableViewState="false"></asp:Label>
                                <asp:TextBox ID="TxtCodCliente_Destinatario_NotaFiscalConsulta" runat="server" MaxLength="10" CssClass="ctxt" Enabled="false" BackColor="WhiteSmoke" onkeypress="return btEnter()" ToolTip="Código do Destinatário. Pesquise pelo nome no campo ao lado caso não saiba o código"></asp:TextBox>
                                <asp:TextBox ID="TxtNome_Destinatario_NotaFiscalConsulta" runat="server" CssClass="ctxt" onkeypress="return btEnter()" TabIndex="60" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaDestinatarioConsultaConsulta()"></asp:TextBox>
                                <asp:TextBox ID="TxtCnpjCpf_Destinatario_NotaFiscalConsulta" Visible="true" Enabled="false" runat="server" CssClass="ctxt" onkeypress="return btEnter()" BackColor="WhiteSmoke"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" TargetControlID="TxtNome_Destinatario_NotaFiscalConsulta" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodigoCnpjCpfPessoa" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDCodigoCnpjCpfDestinatarioConsulta" ContextKey="Cliente,CnpjCpf" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" runat="server" TargetControlID="TxtNome_Destinatario_NotaFiscalConsulta" WatermarkCssClass="ctxtMD" WatermarkText="Pesquise aqui por Código, Nome, CNPJ ou CPF..." />
                                <%--Coleta Consulta--%>
                                <asp:Label ID="LblLocalColetaConsulta" runat="server" Text="Local Coleta" EnableViewState="false" ToolTip="Local da Coleta" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtCodMunicipio_MunicipioColeta_NotaFiscalConsulta" Enabled="false" BackColor="WhiteSmoke" runat="server" ToolTip="Pesquise aqui por Local da Coleta" MaxLength="10" CssClass="ctxt"></asp:TextBox>
                                <asp:TextBox ID="TxtDescMunicipio_MunicipioColeta_NotaFiscalConsulta" runat="server" ToolTip="Pesquise aqui por Local da Coleta" TabIndex="70" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt" OnkeyUp="return LimpaLocalColetaNFConsulta()"></asp:TextBox>
                                <asp:TextBox ID="TxtUF_MunicipioColeta_NotaFiscalConsulta" runat="server" CssClass="ctxt" Enabled="false" BackColor="WhiteSmoke" onkeypress="return btEnter()"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" TargetControlID="TxtDescMunicipio_MunicipioColeta_NotaFiscalConsulta" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaMunicipioCodigoNome" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDMunicipioColetaConsulta" ContextKey="Municipio,DescMunicipio" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender8" runat="server" TargetControlID="TxtDescMunicipio_MunicipioColeta_NotaFiscalConsulta" WatermarkCssClass="ctxtMD" WatermarkText="Pesquise aqui por Código ou Local de Coleta..." />
                                <%--Coleta--%>
                                <%--Entrega Consulta --%>
                                <asp:Label ID="LblLocalEntregaConsulta" runat="server" Text="Local Entrega" EnableViewState="false" ToolTip="Local da Entrega" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtCodMunicipio_MunicipioEntrega_NotaFiscalConsulta" Enabled="false" BackColor="WhiteSmoke" runat="server" ToolTip="Pesquise aqui por Local da Entrega" MaxLength="10" CssClass="ctxt"></asp:TextBox>
                                <asp:TextBox ID="TxtDescMunicipio_MunicipioEntrega_NotaFiscalConsulta" runat="server" ToolTip="Pesquise aqui por Local da Coleta" TabIndex="80" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt" OnkeyUp="return LimpaLocalEntregaNFConsulta()"></asp:TextBox>
                                <asp:TextBox ID="TxtUF_MunicipioEntrega_NotaFiscalConsulta" runat="server" CssClass="ctxt" Enabled="false" BackColor="WhiteSmoke" onkeypress="return btEnter()"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" TargetControlID="TxtDescMunicipio_MunicipioEntrega_NotaFiscalConsulta" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaMunicipioCodigoNome" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDMunicipioEntregaConsulta" ContextKey="Municipio,DescMunicipio" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender9" runat="server" TargetControlID="TxtDescMunicipio_MunicipioEntrega_NotaFiscalConsulta" WatermarkCssClass="ctxtMD" WatermarkText="Pesquise aqui por  Código ou Local da Entrega..." />
                                <%--Entrega--%>
                                <asp:Label ID="LblNaturezaConsulta" runat="server" Text="Natureza" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtCodNatureza_Natureza_NotaFiscalConsulta" runat="server" Enabled="false" BackColor="WhiteSmoke" CssClass="ctxt"></asp:TextBox>
                                <asp:TextBox ID="TxtDescNatureza_Natureza_NotaFiscalConsulta" runat="server" CssClass="ctxt" TabIndex="90" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaNaturezaConsulta()"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" TargetControlID="TxtDescNatureza_Natureza_NotaFiscalConsulta" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodDescr" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDNaturezaCodDescConsulta" ContextKey="Natureza,DescNatureza" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender10" runat="server" TargetControlID="TxtDescNatureza_Natureza_NotaFiscalConsulta" WatermarkCssClass="ctxtMD" WatermarkText="Pesquise aqui por Codigo ou Descrição da Natureza..." />
                                <asp:Label ID="LblEspecieConsulta" runat="server" Text="Espécie" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtCodEspecie_Especie_NotaFiscalConsulta" runat="server" Enabled="false" BackColor="WhiteSmoke" CssClass="ctxt" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:TextBox>
                                <asp:TextBox ID="TxtDescEspecie_Especie_NotaFiscalConsulta" runat="server" CssClass="ctxt" TabIndex="100" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaEspecieConsulta()"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" TargetControlID="TxtDescEspecie_Especie_NotaFiscalConsulta" CompletionInterval="8" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodDescr" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDEspecieCodDescrConsulta" ContextKey="Especie,DescEspecie" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender11" runat="server" TargetControlID="TxtDescEspecie_Especie_NotaFiscalConsulta" WatermarkCssClass="ctxtMD" WatermarkText="Pesquise aqui por Codigo ou Descrição da Espécie..." />
                                <asp:Label ID="LblChaveNFeConsulta" runat="server" Text="Chave NF-e" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtChaveEletronicaNFe_NotaFiscalConsulta" runat="server" TabIndex="110" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt" MaxLength="44"></asp:TextBox>
                                <%--Numero Carga--%>
                                <asp:Label ID="LblNumeroCargaNF" runat="server" Text="Nº Carga" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtNumeroCarga_NotaFiscalConsulta" runat="server" MaxLength="10" TabIndex="20" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt"></asp:TextBox>
                                <%--Numero Carga--%>
                                <%-- CIF / FOB Consulta --%>
                                <asp:Label ID="LblCifFobConsulta" runat="server" Text="CIF / FOB" EnableViewState="false" ToolTip="CIF / FOB" CssClass="lbldescricao"></asp:Label>
                                <asp:DropDownList ID="DdlCifFobConsulta" runat="server" ToolTip="CIF / FOB" CssClass="drolist" TabIndex="120">
                                    <asp:ListItem Text="Todos" Value=""></asp:ListItem>
                                    <asp:ListItem Text="CIF" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="FOB" Value="F"></asp:ListItem>
                                </asp:DropDownList>
                                <%-- CIF / FOB --%>
                                <asp:Button ID="BtnConsultarNotaFiscal" runat="server" CssClass="BotaoToolbox" TabIndex="970" Text="Consultar" ToolTip="Clique aqui para consultar de acordo com os parametros informados" OnClick="BtnConsultarNotaFiscal_Click" />
                                <asp:Button ID="BtnLimparConsultaNF" runat="server" CssClass="BotaoToolbox" TabIndex="970" Text="Limpar" ToolTip="Clique aqui para consultar de acordo com os parametros informados" OnClick="BtnLimparConsultaNotaFiscal_Click" />
                                <asp:Button ID="BtnCancelarConsulta" runat="server" CssClass="BotaoToolbox" TabIndex="970" Text="Cancelar" ToolTip="Clique aqui para voltar a geração do CTRC" OnClick="BtnAbortarConsultaNF_Click" />
                            </asp:View>
                            <%--Realiza consultas de Ct-e de subContratação--%>
                            <asp:View ID="viewConsultaConheciemento" runat="server">
                                <div id="DivConsultarCteTerceiro">
                                    <asp:Label ID="LblConsultarCteTerceiro" runat="server" CssClass="lbldescricao" Text="Informe Opções de Filtro do Ct-e Terceiro"></asp:Label>
                                </div>
                                <div id="DivCabecalhoModalCTETerceiro"></div>
                                <asp:Label ID="LblTipoConsultaCteTerceiro" runat="server" Text="Consultar" CssClass="lbldescricao"></asp:Label>
                                <asp:DropDownList ID="DdlTipoConsultaCteTerceiro" runat="server" CssClass="drolistObr" onkeydown="return ModifyEnterKeyPressAsTab()" TabIndex="10"></asp:DropDownList>
                                <asp:Label ID="LblFilialConsultaCteTerceiro" runat="server" Text="Filial" CssClass="lbldescricao"></asp:Label>
                                <asp:DropDownList ID="DdlIdFilial_Filial_CteTerceiroConsulta" runat="server" CssClass="drolist" TabIndex="10" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:DropDownList>
                                <asp:Label ID="LblCTETerceiroConsulta" runat="server" Text="Nº CT-e Terceiro" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtNumeroCTeTerceiro_CteTerceiroConsulta" runat="server" MaxLength="10" TabIndex="20" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt" alt="integerde10"></asp:TextBox>
                                <asp:Label ID="LblDataEmissaoConsultaInicialCteTerceiro" runat="server" Text="Emissão" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtDataEmissaoConsultaInicialCteTerceiro" runat="server" MaxLength="10" CssClass="ctxt" TabIndex="30" onkeydown="return ModifyEnterKeyPressAsTab()"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtenderConsultaInicialCteTerceiro" TargetControlID="TxtDataEmissaoConsultaInicialCteTerceiro" MaskType="Date" Mask="99/99/9999" runat="server" />
                                <input id="BtnCalendarioConsultaDataEmissaoInicialCteTerceiro" type="button" value=" " visible="true" class="BotaoCalendario" />
                                <cc1:CalendarExtender ID="CalendarExtender4" TargetControlID="TxtDataEmissaoConsultaInicialCteTerceiro" PopupButtonID="BtnCalendarioConsultaDataEmissaoInicialCteTerceiro" PopupPosition="BottomLeft" CssClass="Calendario" runat="server" />
                                <asp:Label ID="LblCodEmitenteConsultaCteTerceiro" runat="server" Text="Emitente" CssClass="lbldescricao" EnableViewState="false"></asp:Label>
                                <asp:TextBox ID="TxtCodCliente_Emitente_CteTerceiroConsulta" runat="server" MaxLength="10" CssClass="ctxt" BackColor="WhiteSmoke" Enabled="false" onkeypress="return btEnter()" ToolTip="Código do Emitente. Pesquise pelo nome no campo ao lado caso não saiba o código"></asp:TextBox>
                                <asp:TextBox ID="TxtNome_Emitente_CteTerceiroConsulta" runat="server" CssClass="ctxt" onkeypress="return btEnter()" TabIndex="50" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaEmitenteConsultaCteTerceiro()"></asp:TextBox>
                                <asp:TextBox ID="TxtCnpjCpf_Emitente_CteTerceiroConsulta" Visible="true" Enabled="false" runat="server" CssClass="ctxt" onkeypress="return btEnter()" BackColor="WhiteSmoke"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender18" runat="server" TargetControlID="TxtNome_Emitente_CteTerceiroConsulta" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodigoCnpjCpfPessoa" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDCodigoCnpjCpfEmitenteConsultaCteTerceiro" ContextKey="Cliente,CnpjCpf" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender18" runat="server" TargetControlID="TxtNome_Emitente_CteTerceiroConsulta" WatermarkCssClass="ctxtMD" WatermarkText="Pesquise aqui por Código, Nome, CNPJ ou CPF..." />
                                <asp:Label ID="LblCodRemetenteConsultaCteTerceiro" runat="server" Text="Remetente" CssClass="lbldescricao" EnableViewState="false"></asp:Label>
                                <asp:TextBox ID="TxtCodCliente_Remetente_CteTerceiroConsulta" runat="server" MaxLength="10" CssClass="ctxt" BackColor="WhiteSmoke" Enabled="false" onkeypress="return btEnter()" ToolTip="Código do Remetente. Pesquise pelo nome no campo ao lado caso não saiba o código"></asp:TextBox>
                                <asp:TextBox ID="TxtNome_Remetente_CteTerceiroConsulta" runat="server" CssClass="ctxt" onkeypress="return btEnter()" TabIndex="50" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaRemetenteConsultaCteTerceiro()"></asp:TextBox>
                                <asp:TextBox ID="TxtCnpjCpf_Remetente_CteTerceiroConsulta" Visible="true" Enabled="false" runat="server" CssClass="ctxt" onkeypress="return btEnter()" BackColor="WhiteSmoke"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender14" runat="server" TargetControlID="TxtNome_Remetente_CteTerceiroConsulta" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodigoCnpjCpfPessoa" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDCodigoCnpjCpfClienteConsultaCteTerceiro" ContextKey="Cliente,CnpjCpf" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender13" runat="server" TargetControlID="TxtNome_Remetente_CteTerceiroConsulta" WatermarkCssClass="ctxtMD" WatermarkText="Pesquise aqui por Código, Nome, CNPJ ou CPF..." />
                                <asp:Label ID="LblCodDestinatarioConsultaCteTerceiro" runat="server" Text="Destinatário" CssClass="lbldescricao" EnableViewState="false"></asp:Label>
                                <asp:TextBox ID="TxtCodCliente_Destinatario_CteTerceiroConsulta" runat="server" MaxLength="10" CssClass="ctxt" Enabled="false" BackColor="WhiteSmoke" onkeypress="return btEnter()" ToolTip="Código do Destinatário. Pesquise pelo nome no campo ao lado caso não saiba o código"></asp:TextBox>
                                <asp:TextBox ID="TxtNome_Destinatario_CteTerceiroConsulta" runat="server" CssClass="ctxt" onkeypress="return btEnter()" TabIndex="60" onkeydown="return ModifyEnterKeyPressAsTab()" OnkeyUp="return LimpaDestinatarioConsultaCteTerceiro()"></asp:TextBox>
                                <asp:TextBox ID="TxtCnpjCpf_Destinatario_CteTerceiroConsulta" Visible="true" Enabled="false" runat="server" CssClass="ctxt" onkeypress="return btEnter()" BackColor="WhiteSmoke"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender15" runat="server" TargetControlID="TxtNome_Destinatario_CteTerceiroConsulta" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaCodigoCnpjCpfPessoa" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDCodigoCnpjCpfDestinatarioConsultaCteTerceiro" ContextKey="Cliente,CnpjCpf" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender14" runat="server" TargetControlID="TxtNome_Destinatario_CteTerceiroConsulta" WatermarkCssClass="ctxtMD" WatermarkText="Pesquise aqui por Código, Nome, CNPJ ou CPF..." />
                                <%-- Ct-e de subContratação--%>
                                <%--Coleta Consulta--%>
                                <asp:Label ID="LblLocalColetaConsultaCteTerceiro" runat="server" Text="Local Coleta" EnableViewState="false" ToolTip="Local da Coleta" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtCodMunicipio_MunicipioColeta_CteTerceiroConsulta" Enabled="false" BackColor="WhiteSmoke" runat="server" ToolTip="Pesquise aqui por Local da Coleta" MaxLength="10" CssClass="ctxt"></asp:TextBox>
                                <asp:TextBox ID="TxtDescMunicipio_MunicipioColeta_CteTerceiroConsulta" runat="server" ToolTip="Pesquise aqui por Local da Coleta" TabIndex="70" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt" OnkeyUp="return LimpaLocalColetaCteTerceiroConsulta()"></asp:TextBox>
                                <asp:TextBox ID="TxtUF_MunicipioColeta_CteTerceiroConsulta" runat="server" CssClass="ctxt" Enabled="false" BackColor="WhiteSmoke" onkeypress="return btEnter()"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender16" runat="server" TargetControlID="TxtDescMunicipio_MunicipioColeta_CteTerceiroConsulta" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaMunicipioCodigoNome" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDMunicipioColetaCteTerceiroConsulta" ContextKey="Municipio,DescMunicipio" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender16" runat="server" TargetControlID="TxtDescMunicipio_MunicipioColeta_CteTerceiroConsulta" WatermarkCssClass="ctxtMD" WatermarkText="Pesquise aqui por Código ou Local de Coleta..." />
                                <%--Coleta--%>
                                <%--Entrega Consulta --%>
                                <asp:Label ID="LblLocalEntregaConsultaCteTerceiro" runat="server" Text="Local Entrega" EnableViewState="false" ToolTip="Local da Entrega" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtCodMunicipio_MunicipioEntrega_CteTerceiroConsulta" Enabled="false" BackColor="WhiteSmoke" runat="server" ToolTip="Pesquise aqui por Local da Entrega" MaxLength="10" CssClass="ctxt"></asp:TextBox>
                                <asp:TextBox ID="TxtDescMunicipio_MunicipioEntrega_CteTerceiroConsulta" runat="server" ToolTip="Pesquise aqui por Local da Coleta" TabIndex="80" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt" OnkeyUp="return LimpaLocalEntregaCTTerceiroConsulta()"></asp:TextBox>
                                <asp:TextBox ID="TxtUF_MunicipioEntrega_CteTerceiroConsulta" runat="server" CssClass="ctxt" Enabled="false" BackColor="WhiteSmoke" onkeypress="return btEnter()"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender17" runat="server" TargetControlID="TxtDescMunicipio_MunicipioEntrega_CteTerceiroConsulta" CompletionInterval="17" Enabled="true" CompletionListCssClass="Consulta" CompletionSetCount="20" ServiceMethod="ConsultaMunicipioCodigoNome" ServicePath="~/WebConsulta/Consultas.asmx" UseContextKey="true" OnClientItemSelected="CompletaIDMunicipioEntregaCTeterceiroConsulta" ContextKey="Municipio,DescMunicipio" MinimumPrefixLength="1" />
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender17" runat="server" TargetControlID="TxtDescMunicipio_MunicipioEntrega_CteTerceiroConsulta" WatermarkCssClass="ctxtMD" WatermarkText="Pesquise aqui por  Código ou Local da Entrega..." />
                                <%--Entrega--%>
                                <%--Chave Cte Terceiro--%>
                                <asp:Label ID="LblChaveCTeTerceiroConsulta" runat="server" Text="Chave CT-e Terc" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtChaveEletronicaCTeTerceiro_CTeTerceiroConsulta" runat="server" TabIndex="110" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt" MaxLength="44"></asp:TextBox>
                                <%--Chave Cte Terceiro--%>
                                <%--Numero Carga--%>
                                <asp:Label ID="LblNumeroCarga" runat="server" Text="Nº Carga" CssClass="lbldescricao"></asp:Label>
                                <asp:TextBox ID="TxtNumeroCarga_CteTerceiroConsulta" runat="server" MaxLength="10" TabIndex="20" onkeydown="return ModifyEnterKeyPressAsTab()" CssClass="ctxt"></asp:TextBox>
                                <%--Numero Carga--%>
                                <%--Botões--%>
                                <asp:Button ID="BtnConsultarCTeTerceiro" runat="server" CssClass="BotaoToolbox" TabIndex="970" Text="Consultar" ToolTip="Clique aqui para consultar de acordo com os parametros informados" OnClick="BtnConsultarCteTerceirol_Click" />
                                <asp:Button ID="BtnLimparConsultaCTeTerceiro" runat="server" CssClass="BotaoToolbox" TabIndex="970" Text="Limpar" ToolTip="Clique aqui para consultar de acordo com os parametros informados" OnClick="BtnLimparConsultaCteTerceiro_Click" />
                                <asp:Button ID="BtnCancelarConsultaCTeTerceiro" runat="server" CssClass="BotaoToolbox" TabIndex="970" Text="Cancelar" ToolTip="Clique aqui para voltar a geração do CTRC" OnClick="BtnAbortarCteTerceiro_Click" />
                                <%--Botões--%>
                            </asp:View>
                        </asp:MultiView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:View>
            <%--View onde se realiza Importacao de XML das notas--%>
            <asp:View ID="viewImportacaoXMLNotas" runat="server">
                <div id="DivLblMensagemImportacaoXML">
                    <asp:Label ID="LblMensagemImportacaoXML" runat="server" EnableViewState="false" Text=""></asp:Label>
                </div>
                <div id="DivImportarNotasXML">
                    <asp:Label ID="LblImportarNotasXML" runat="server" CssClass="lbldescricao" Text="Importação de XML de Notas Fiscais"></asp:Label>
                </div>
                <asp:Panel ID="pnlCabecalho" runat="server" Enabled="true">
                    <asp:Label ID="LblFilialXML" runat="server" Text="Filial" CssClass="lbldescricao"></asp:Label>
                    <asp:DropDownList ID="DdlIdFilial_Filial_XML" runat="server" CssClass="drolistObr" TabIndex="1"></asp:DropDownList>
                    <asp:LinkButton ID="LkbAtualizarFilialXML" runat="server" CssClass="lkbatualizar" TabIndex="24" EnableViewState="false" OnClick="LkbAtualizarFilial_Click"></asp:LinkButton>
                    <asp:LinkButton ID="LkbEditarFilialXML" runat="server" CssClass="lkbeditar" TabIndex="26" EnableViewState="false" OnClick="LkbEditarFilialXML_Click"></asp:LinkButton>
                    <div id="DivAtualizarDadosCadastrais">
                        <asp:CheckBox ID="chkAtualizarDadosCadastrais" runat="server" CssClass="chkb" Text="Atualizar dados cadastrais dos clientes" />
                    </div>
                    <asp:Label ID="LblTipoNotaXML" runat="server" Text="Tipo de Nota" CssClass="lbldescricao"></asp:Label>
                    <asp:DropDownList ID="DdlTipoNotaXML" runat="server" CssClass="drolistObr" TabIndex="2"></asp:DropDownList>
                    <asp:Panel ID="PnlNaturezaXML" runat="server" Visible="true">
                        <asp:Label ID="LblNaturezaXML" runat="server" Text="Natureza" CssClass="lbldescricao"></asp:Label>
                        <asp:DropDownList ID="DdlIdNatureza_Natureza_XML" runat="server" CssClass="drolistObr" TabIndex="3"></asp:DropDownList>
                        <asp:LinkButton ID="LkbAtualizarNaturezaXML" runat="server" CssClass="lkbatualizar" TabIndex="24" EnableViewState="false" OnClick="LkbAtualizarNatureza_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LkbEditarNaturezaXML" runat="server" CssClass="lkbeditar" TabIndex="26" EnableViewState="false" OnClick="LkbEditarNaturezaXML_Click"></asp:LinkButton>
                    </asp:Panel>
                    <asp:Panel ID="PnlEspecieXML" runat="server" Visible="true">
                        <asp:Label ID="LblEspecieXML" runat="server" Text="Espécie" CssClass="lbldescricao"></asp:Label>
                        <asp:DropDownList ID="DdlIdEspecie_Especie_XML" runat="server" CssClass="drolistObr" TabIndex="3"></asp:DropDownList>
                        <asp:LinkButton ID="LkbAtualizarEspecieXML" runat="server" CssClass="lkbatualizar" TabIndex="24" EnableViewState="false" OnClick="LkbAtualizarEspecie_Click"></asp:LinkButton>
                        <asp:LinkButton ID="LkbEditarEspecieXML" runat="server" CssClass="lkbeditar" TabIndex="26" EnableViewState="false" OnClick="LkbEditarEspecieXML_Click"></asp:LinkButton>
                    </asp:Panel>
                </asp:Panel>
                <div id="DivSelecao">
                    <asp:Label ID="LblSelecao" runat="server" Text="Selecione o(s) arquivo(s) para importação" CssClass="lbldescricao"></asp:Label>
                </div>
                <div class="uploadotimizado" id="divupload" runat="server">
                    <input id="fileSelector" type="file" runat="server" visible="false" accept="xml/*" />
                    <asp:FileUpload ID="FileUpload1" runat="server" multiple accept="xml/*" />
                    <asp:TextBox ID="TxtListagemArquivo" runat="server" CssClass="ctxt" TabIndex="770" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                </div>
                <asp:Button ID="BtnListarXML" runat="server" Text="Listar XMLs Selecionados" ToolTip="Clique aqui para listar XML escolhidos" OnClick="BtnListarXML_Click" UseSubmitBehavior="false" />
                <asp:Label ID="LblListarXML" runat="server" Text="" CssClass="lbldescricao"></asp:Label>
                <asp:Button ID="BtnNovoImportacaoXML" runat="server" Text="Importar XML" ToolTip="Clique aqui para nova importação" OnClick="BtnImportar_Click" UseSubmitBehavior="false" />
                <asp:Button ID="BtnCancelarImportacaoXML" runat="server" CssClass="BotaoToolbox" TabIndex="970" Text="Cancelar" ToolTip="Clique aqui para importar os XMLs escolhidos" OnClick="BtnAbortarImportacaoXML_Click" />
            </asp:View>
            <%--View onde se realiza Importacao de XML do CT-es de Terceiros--%>
            <asp:View ID="viewImportacaoXMLCTeTerceiro" runat="server">
                <div id="DivLblMensagemImportacaoXMLCTeTerceiro">
                    <asp:Label ID="LblMensagemImportacaoXMLCTeTerceiro" runat="server" EnableViewState="false" Text=""></asp:Label>
                </div>
                <div id="DivImportarNotasXMLCTeTerceiro">
                    <asp:Label ID="LblImportarNotasXMLCTeTerceiro" runat="server" CssClass="lbldescricao" Text="Importação de XML - CT-e Terceiro"></asp:Label>
                </div>
                <asp:Panel ID="pnlCabecalhoCTeTeceiro" runat="server" Enabled="true">
                    <asp:Label ID="LblFilialXMLCTeTerceiro" runat="server" Text="Filial" CssClass="lbldescricao"></asp:Label>
                    <asp:DropDownList ID="DdlIdFilial_Filial_XMLCTeTerceiro" runat="server" CssClass="drolistObr" TabIndex="1"></asp:DropDownList>
                    <asp:LinkButton ID="LkbAtualizarFilialXMLCTeTerceiro" runat="server" CssClass="lkbatualizar" TabIndex="24" EnableViewState="false" OnClick="LkbAtualizarFilial_Click"></asp:LinkButton>
                    <asp:LinkButton ID="LkbEditarFilialXMLCTeTerceiro" runat="server" CssClass="lkbeditar" TabIndex="26" EnableViewState="false" OnClick="LkbEditarFilialXML_Click"></asp:LinkButton>
                </asp:Panel>
                <div id="DivSelecaoCTeTerceiro">
                    <asp:Label ID="LblSelecaoCTeTerceiro" runat="server" Text="Selecione o(s) arquivo(s) para importação" CssClass="lbldescricao"></asp:Label>
                </div>
                <div class="uploadotimizado" id="divuploadCTeTerceiro" runat="server">
                    <input id="fileSelectorCTeTerceiro" type="file" runat="server" visible="false" accept="xml/*" />
                    <asp:FileUpload ID="FileUpload2" runat="server" multiple accept="xml/*" />
                    <asp:TextBox ID="TxtListagemArquivoCTeTerceiro" runat="server" CssClass="ctxt" TabIndex="770" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                </div>
                <asp:Button ID="BtnListarXMLCTeTerceiro" runat="server" Text="Listar XMLs Selecionados" ToolTip="Clique aqui para listar XML escolhidos" OnClick="BtnListarXML_Click" UseSubmitBehavior="false" />
                <asp:Label ID="LblListarXMLCTeTerceiro" runat="server" Text="" CssClass="lbldescricao"></asp:Label>
                <asp:Button ID="BtnNovoImportacaoXMLCTeTerceiro" runat="server" Text="Importar XML" ToolTip="Clique aqui para nova importação" OnClick="BtnImportar_Click" UseSubmitBehavior="false" />
                <asp:Button ID="BtnCancelarImportacaoXMLCTeTerceiro" runat="server" CssClass="BotaoToolbox" TabIndex="970" Text="Cancelar" ToolTip="Clique aqui para importar os XMLs escolhidos" OnClick="BtnAbortarImportacaoXML_Click" />
            </asp:View>
        </asp:MultiView>
        <%-- Início DO MODAL Tabela de Frete --%>
        <a href="#" style="display: none; visibility: hidden;" onclick="return false" id="LinkOcultoTabelaFrete" runat="server"></a>
        <div id="ModalTabelaFrete" class="ModalFiltro" onload="ListarTabelaFrete();">
            <asp:Panel ID="PnlTabTabelaFrete" runat="server" CssClass="PnlFiltro" Style="display: none;">
                <asp:Panel runat="server" ID="ToolBoxModalTabelaFrete" CssClass="ToolBoxModal">
                    <asp:Button ID="BtnTeste" runat="server" CssClass="BotaoToolboxProximo"
                        OnClick="ListarRegistrosTabelaFrete_Click" Style="display: none" />
                    <asp:Label runat="server" CssClass="lbltitulo" ID="LblTituloTabelaFrete" Text="Tabela de Frete"></asp:Label>
                    <asp:Button ID="BtnFecharModalTabelaFrete" runat="server" CssClass="BotaoToolboxProximo" Text="Fechar"
                        OnClientClick="BtnFecharModalTabelaFrete_Click" />
                </asp:Panel>
                <asp:Label ID="LblMensagemModalTabelaFrete" runat="server" CssClass="lbldescricao" Text=""></asp:Label>
                <div id="DivListaGridTabelaFrete" onload="ListarTabelaFrete();">
                    <asp:GridView ID="GvwListaTabelaFrete" runat="server" Width="100%" CellSpacing="1" CellPadding="0" GridLines="None" AutoGenerateColumns="false">
                        <RowStyle CssClass="primeiroRegistro" />
                        <HeaderStyle CssClass="headerEstilo" />
                        <PagerStyle CssClass="paginacaoEstilo" />
                        <AlternatingRowStyle CssClass="segundoRegistro" />
                        <Columns>
                            <asp:TemplateField HeaderText="selecione" Visible="true" ItemStyle-Width="10px">
                                <ItemTemplate>
                                    <div id="divselect" runat="server">
                                        <asp:CheckBox ID="chkselect" runat="server" onClick="SelecionarCheck(this, 'Normal')" />
                                    </div>
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <input id="chkall" onclick="javascript: selecionatodoschecks(this);" runat="server" type="checkbox" />
                                </HeaderTemplate>
                                <ItemStyle Width="10px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IdTabelaFrete" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="IdTabelaFrete" runat="server" Visible="false" Text='<%# Eval("IdTabelaFrete") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="DescTipoTabela" runat="server" Text='<%# Eval("DescTipoTabela") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Transporte" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" SortExpression="DescTipoTransporte">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="DescTipoTransporte" runat="server" Text='<%# Eval("DescTipoTransporte") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cliente" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" SortExpression="DescCliente">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemStyle Width="22%" />
                                <ItemTemplate>
                                    <asp:Label ID="DescCliente" runat="server" Text='<%# Eval("DescCliente") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Origem" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                                ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" SortExpression="DescLocalOrigem">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemStyle Width="14%" />
                                <ItemTemplate>
                                    <asp:Label ID="DescLocalOrigem" runat="server" Text='<%# Eval("DescLocalOrigem") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Destino" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                                ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" SortExpression="DescLocalDestino">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemStyle Width="14%" />
                                <ItemTemplate>
                                    <asp:Label ID="DescLocalDestino" runat="server" Text='<%# Eval("DescLocalDestino") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Espécie" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" SortExpression="DescEspecie">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemStyle Width="14%" />
                                <ItemTemplate>
                                    <asp:Label ID="DescEspecie" runat="server" Text='<%# Eval("DescEspecie") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Natureza" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false"
                                SortExpression="DescNatureza">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemStyle Width="14%" />
                                <ItemTemplate>
                                    <asp:Label ID="DescNatureza" runat="server" Text='<%# Eval("DescNatureza") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vigência" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" SortExpression="Validade">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="Validade" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}", Eval("Validade")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:ImageButton ID="BtnSelecionarTabelaFrete" runat="server" ImageUrl="~/Imagem/setaDown.png" OnCommand="SelecionarTabelaFrete_Click"
                    Width="15px" ToolTip="Selecionar Tabela de Frete" />
                <asp:ImageButton ID="BtnRemoverTabelaFrete" runat="server" ImageUrl="~/Imagem/setaUp.png" OnCommand="RemoverTabelaFrete_Click"
                    Width="15px" ToolTip="Remover Tabela de Frete" />
                <div id="DivListaGridTabelaFreteSelecionada">
                    <asp:GridView ID="GvwTabelaFreteSelecionada" runat="server" Width="100%" CellSpacing="1" CellPadding="0" GridLines="None"
                        AutoGenerateColumns="false">
                        <RowStyle CssClass="primeiroRegistro" />
                        <HeaderStyle CssClass="headerEstilo" />
                        <PagerStyle CssClass="paginacaoEstilo" />
                        <AlternatingRowStyle CssClass="segundoRegistro" />
                        <Columns>
                            <asp:TemplateField HeaderText="selecione" Visible="true" ItemStyle-Width="10px">
                                <ItemTemplate>
                                    <div id="divselect" runat="server">
                                        <asp:CheckBox ID="chkselect" runat="server" onClick="SelecionarCheck(this, 'Normal')" />
                                    </div>
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <input id="chkall" onclick="javascript: selecionatodoschecks(this);" runat="server" type="checkbox" />
                                </HeaderTemplate>
                                <ItemStyle Width="10px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IdTabelaFrete" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="IdTabelaFrete" runat="server" Visible="false" Text='<%# Eval("IdTabelaFrete") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="DescTipoTabela" runat="server" Text='<%# Eval("DescTipoTabela") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Transporte" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" SortExpression="DescTipoTransporte">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="DescTipoTransporte" runat="server" Text='<%# Eval("DescTipoTransporte") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cliente" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" SortExpression="DescCliente">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemStyle Width="22%" />
                                <ItemTemplate>
                                    <asp:Label ID="DescCliente" runat="server" Text='<%# Eval("DescCliente") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Origem" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                                ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" SortExpression="DescLocalOrigem">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemStyle Width="14%" />
                                <ItemTemplate>
                                    <asp:Label ID="DescLocalOrigem" runat="server" Text='<%# Eval("DescLocalOrigem") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Destino" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                                ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" SortExpression="DescLocalDestino">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemStyle Width="14%" />
                                <ItemTemplate>
                                    <asp:Label ID="DescLocalDestino" runat="server" Text='<%# Eval("DescLocalDestino") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Espécie" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" SortExpression="DescEspecie">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemStyle Width="14%" />
                                <ItemTemplate>
                                    <asp:Label ID="DescEspecie" runat="server" Text='<%# Eval("DescEspecie") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Natureza" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false"
                                SortExpression="DescNatureza">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemStyle Width="14%" />
                                <ItemTemplate>
                                    <asp:Label ID="DescNatureza" runat="server" Text='<%# Eval("DescNatureza") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vigência" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" SortExpression="Validade">
                                <ItemStyle HorizontalAlign="Center" />

                                <ItemTemplate>
                                    <asp:Label ID="Validade" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}", Eval("Validade")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="MpeTabelaFrete" runat="server" TargetControlID="LinkOcultoTabelaFrete"
                PopupDragHandleControlID="ToolBoxModalTabelaFrete" PopupControlID="PnlTabTabelaFrete"
                BackgroundCssClass="modalBackground" DropShadow="true" />
        </div>
        <%-- Fim Do MODAL Tabela de Frete --%>
        <%-- INICIO MODAL Composição de Peso Cubado--%>
        <a href="#" style="display: none; visibility: hidden;" onclick="return false" id="LinkOcultoComposicaoPesoCubado" runat="server"></a>
        <div id="ModalComposicaoPesoCubado" class="ModalFiltro">
            <asp:Panel ID="PnlComposicaoPesoCubado" runat="server" CssClass="PnlFiltro" Style="display: none;">
                <asp:Panel runat="server" ID="ToolBoxComposicaoPesoCubado" CssClass="ToolBoxModal">
                    <asp:Label runat="server" CssClass="lbltitulo" ID="LblModalComposicaoPesoCubado" Text="Composição de Peso M³"></asp:Label>
                </asp:Panel>
                <asp:Label ID="LblAlturaComposicao" runat="server" CssClass="lbldescricao" Text="Altura"></asp:Label>
                <asp:TextBox ID="TxtAlturaComposicao" runat="server" CssClass="ctxt" alt="decimal10de4" onblur="return CalculaVolumeComposicao()"></asp:TextBox>
                <asp:Label ID="LblLarguraComposicao" runat="server" CssClass="lbldescricao" Text="Largura"></asp:Label>
                <asp:TextBox ID="TxtLarguraComposicao" runat="server" CssClass="ctxt" alt="decimal10de4" onblur="return CalculaVolumeComposicao()"></asp:TextBox>
                <asp:Label ID="LblComprimentoComposicao" runat="server" CssClass="lbldescricao" Text="Comprimento"></asp:Label>
                <asp:TextBox ID="TxtComprimentoComposicao" runat="server" CssClass="ctxt" alt="decimal10de4" onblur="return CalculaVolumeComposicao()"></asp:TextBox>
                <asp:Label ID="LblFatorCubagenComposicao" runat="server" CssClass="lbldescricao" Text="Fator de Cubagem"></asp:Label>
                <asp:TextBox ID="TxtFatorCubagenComposicao" runat="server" CssClass="ctxt" alt="decimal10de4"></asp:TextBox>
                <asp:Label ID="LblVolumeComposicao" runat="server" CssClass="lbldescricao" Text="Volume"></asp:Label>
                <asp:TextBox ID="TxtVolumeComposicao" Enabled="false" runat="server" CssClass="ctxt" alt="decimal10de4"></asp:TextBox>
                <asp:Button ID="BtnConfirmarComposicao" runat="server" CssClass="BotaoToolbox" OnClientClick="return CalculaPesoCubado();" OnClick="BtnConfirmarComposicao_Click" Text="Confirmar" />
                <asp:HiddenField ID="HidReferencia" runat="server" />
                <asp:HiddenField ID="HidTargetControl" runat="server" />
            </asp:Panel>
            <cc1:ModalPopupExtender ID="MpeComposicaoPesoCubado" runat="server" TargetControlID="LinkOcultoComposicaoPesoCubado"
                PopupDragHandleControlID="ToolBoxComposicaoPesoCubado" PopupControlID="PnlComposicaoPesoCubado" BackgroundCssClass="modalBackground" DropShadow="true" />
        </div>
        <%-- FIM DO MODAL Composição de Peso Cubado--%>

        <%-- INICIO MODAL Composição de Peso Cubado--%>
        <a href="#" style="display: none; visibility: hidden;" onclick="return false" id="LinkOcultoComposicaoCubagem" runat="server"></a>
        <div id="ModalComposicaoCubagem" class="ModalFiltro">

            <asp:Panel ID="PnlTabComponentes" runat="server" CssClass="PnlFiltro" Style="display: none;">
                <asp:Panel runat="server" ID="ToolBoxModalCubagem" CssClass="ToolBoxModal">
                    <asp:Label runat="server" CssClass="lbltitulo" ID="LblModalCubagem" Text="Cubagem"></asp:Label>
                    <asp:Button ID="BtnSalvarCubagem" runat="server" CssClass="BotaoToolboxProximo" Text="Salvar" OnClick="BtnSalvarCubagem_Click" />
                    <asp:Button ID="BtnFecharModalCubagem" runat="server" CssClass="BotaoToolboxProximo" Text="Fechar" OnClick="BtnFecharModalCubagem_Click" />
                </asp:Panel>
                <div id="DivComponenteOperacional"></div>
                <asp:Label runat="server" CssClass="lbldescricao" ID="LblBotoesAddRemove" Text="Valores de Cubagem"></asp:Label>
                <asp:Button ID="BtnAdicionaComponente" runat="server" CssClass="BotaoToolboxProximo" Text="Adicionar" OnClick="BtnAdicionarComponente_Click" />
                <asp:Button ID="BtnRemoveComponente" runat="server" CssClass="BotaoToolboxProximo" Text="Remover" OnClick="BtnRemoverComponente_Click" />
                <asp:Label runat="server" CssClass="lbldescricao" ID="LblTotalCubagem" Text="Total Cubagem"></asp:Label>
                <asp:Label runat="server" CssClass="lbldescricao" ID="LblValorTotalCubagem" Text="Total Cubagem"></asp:Label>
                <asp:Button ID="BtnCalcularComponentePesoCubado" runat="server" CssClass="BotaoToolboxProximo" Text="Calcular" OnClick="BtnCalcularComponentePesoCubado_Click" />
                <div id="DivConhecimentoComponentesCubagem">
                    <asp:GridView ID="GvwConhecimentoComponentesCubagem" runat="server" Width="100%" CellSpacing="1" CellPadding="0" GridLines="None" AutoGenerateColumns="false" OnRowDataBound="GvwListaConhecimentoComponentesCubagem_RowDataBound" DataKeyNames="IdConhecimentoComponentesCubagem"
                        OnPreRender="GvwConhecimentoComponentesCubagem_PreRender">
                        <RowStyle CssClass="primeiroRegistro" />
                        <HeaderStyle CssClass="headerEstilo" />
                        <PagerStyle CssClass="paginacaoEstilo" />
                        <AlternatingRowStyle CssClass="segundoRegistro" />
                        <Columns>
                            <asp:TemplateField HeaderText="selecione" Visible="true" ItemStyle-Width="10px">
                                <ItemTemplate>
                                    <div id="divselect" runat="server">
                                        <asp:CheckBox ID="chkselect" runat="server" onClick="SelecionarCheck(this, 'Normal')" />
                                    </div>
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <input id="chkall" onclick="javascript: selecionatodoschecks(this);" runat="server" type="checkbox" />
                                </HeaderTemplate>
                                <ItemStyle Width="10px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IdConhecimentoComponentesCubagem" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="IdConhecimentoComponentesCubagem" runat="server" Visible="false" Text='<%# Eval("IdConhecimentoComponentesCubagem") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IdCTRC" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="IdCTRC" runat="server" Visible="false" Text='<%# Eval("IdCTRC") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantidade" Visible="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" SortExpression="LimitePeso">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQuantidade_ConhecimentoComponentesCubagem" runat="server" Width="100px" Visible="true" CssClass="ctxt" alt="decimal15de4" onkeydown="return ModifyEnterKeyPressAsTab()" Text='<%# Eval("Quantidade") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Altura" Visible="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" SortExpression="LimiteValor">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAltura_ConhecimentoComponentesCubagem" runat="server" Width="100px" Visible="true" CssClass="ctxt" alt="decimal15de4" onkeydown="return ModifyEnterKeyPressAsTab()" Text='<%# String.Format("{0:C4}",Eval("Altura")) %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Largura" Visible="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" SortExpression="LimiteValor">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtLargura_ConhecimentoComponentesCubagem" runat="server" Width="100px" Visible="true" CssClass="ctxt" alt="decimal15de4" onkeydown="return ModifyEnterKeyPressAsTab()" Text='<%# String.Format("{0:C4}", Eval("Largura")) %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comprimento" Visible="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" SortExpression="LimiteValor">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtComprimento_ConhecimentoComponentesCubagem" runat="server" Width="100px" Visible="true" CssClass="ctxt" alt="decimal15de4" onkeydown="return ModifyEnterKeyPressAsTab()" Text='<%# String.Format("{0:C4}", Eval("Comprimento")) %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fator Cubagem" Visible="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" SortExpression="LimiteValor">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFatorCubagem_ConhecimentoComponentesCubagem" runat="server" Width="100px" Visible="true" CssClass="ctxt" alt="decimal15de4" onkeydown="return ModifyEnterKeyPressAsTab()" Text='<%# String.Format("{0:C4}", Eval("FatorCubagem")) %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Peso Cubado" Visible="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" SortExpression="LimiteValor">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTotalCubagem_ConhecimentoComponentesCubagem" runat="server" Width="100px" Visible="true" Enabled="false" CssClass="ctxt" alt="decimal15de4" onkeydown="return ModifyEnterKeyPressAsTab()" Text='<%# String.Format("{0:C4}", Eval("TotalCubagem")) %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="MpeComposicaoCubagem" runat="server" TargetControlID="LinkOcultoComposicaoCubagem"
                PopupDragHandleControlID="ToolBoxComposicaoPesoCubado" PopupControlID="PnlTabComponentes" BackgroundCssClass="modalBackground" DropShadow="true" />
        </div>
        <%-- FIM DO MODAL Composição de Peso Cubado--%>
        <updateProgress:UpdateProgress ID="UpdateProgress1" runat="server" />
    </form>
</body>
</html>
