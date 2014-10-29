<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DictionaryItemSearch.ascx.cs" Inherits="tooorangey.DictionaryItemKeySearch.UserControls.DictionaryItemSearch" %>
<div style="padding:15px 15px 10px;">
<h3>Dictionary Item 'Key Name' Search By Value</h3>
<p>Find the dictionary key name for the value written out in the webpages</p>
<dl>
    <dt><asp:Label runat="server" ID="lblLanguage" AssociatedControlID="ddlLanguage">Choose Language:</asp:Label></dt>
    <dd><asp:DropDownList runat="server" ID="ddlLanguage"/></dd>
    <dt><asp:Label runat="server" ID="lblSearchTerm" AssociatedControlID="txtSearchTerm">Search Text:</asp:Label></dt>
    <dd><asp:TextBox runat="server" ID="txtSearchTerm"></asp:TextBox></dd>
</dl>
<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_OnClick"/>

<asp:Literal runat="server" ID="litStatusMessage" EnableViewState="false" Visible="False"></asp:Literal>
<asp:ListView runat="server" ID="lvSearchResults" ItemPlaceholderID="placeHolder">
 <LayoutTemplate >
     
        <table>
        <tr><th>Dictionary Key</th><th>Dictionary Value</th><th>Edit</th></tr>
        <asp:PlaceHolder runat="server" ID="placeHolder"></asp:PlaceHolder>

    </table>

 </LayoutTemplate>
    <ItemTemplate>
        <tr><td><%# Eval("key") %></td><td><%#GetValue(Eval("key").ToString()) %></td><td><asp:LinkButton runat="server" ID="lnkButton" CommandArgument='<%#Eval("key") %>' CommandName="editdicitem" OnCommand="lnkButton_OnCommand" Text="Edit"></asp:LinkButton></td></tr>
    </ItemTemplate>
    

</asp:ListView>

    </div>