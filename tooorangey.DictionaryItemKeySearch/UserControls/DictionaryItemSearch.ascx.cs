using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.UI;
using System.Web.UI.WebControls;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;
using umbraco.editorControls;
using Umbraco.Web;
using umbraco.webservices;
using umbraco.cms.businesslogic;
using Language = umbraco.cms.businesslogic.language.Language;

namespace tooorangey.DictionaryItemKeySearch.UserControls
{
    public partial class DictionaryItemSearch : UserControl
    {
        public int SelectedLanguage
        {
            get { return Int32.Parse(ddlLanguage.SelectedValue); }
            set { ddlLanguage.SelectedValue = value.ToString(); }
        }
        protected List<Dictionary.DictionaryItem> AllDictionaryItems = new List<Dictionary.DictionaryItem>();
        protected List<Dictionary.DictionaryItem> MatchingItems = new List<Dictionary.DictionaryItem>();
        protected List<Dictionary.DictionaryItem> ContainingItems = new List<Dictionary.DictionaryItem>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // get all languages in the umbraco site
                var allLanguages = Language.GetAllAsList().ToList();
                var currentContext = UmbracoContext.Current;
                // get current back office user
               var currentUser = currentContext.UmbracoUser;
            
                // make a guess at the default culture to show for this user
                var currentUmbracoLanguage = new Language(1);
                foreach (var language in allLanguages)
                {
                    if (language.CultureAlias.Contains(currentUser.Language))
                    {
                        currentUmbracoLanguage = language;
                    }
                }
                // create a dropdown for the user to select which language they want to search
                ddlLanguage.DataSource = allLanguages.OrderBy(f => f.FriendlyName);
                ddlLanguage.DataTextField = "FriendlyName";
                ddlLanguage.DataValueField = "id";
                ddlLanguage.DataBind();
                ddlLanguage.SelectedValue = currentUmbracoLanguage.id.ToString();
            }


        }

        /// <summary>
        /// Loop through all the dictionary items and add the to a List
        /// probably not all that efficient this
        /// </summary>
        private void PopulateAllDictionaryItems()
        {
            foreach (Dictionary.DictionaryItem item in Dictionary.getTopMostItems)
            {
                this.AddToList(item, ref this.AllDictionaryItems);
            }

        }

        /// <summary>
        /// Set the dictionary Key in the DictionaryDashboard and trigger the change
        /// </summary>
        /// <param name="key"></param>
        private void SetDictionaryKey(string key)
        {

            List<DropDownList> dropdownControls = new List<DropDownList>();
            FindChildControlsRecursive<DropDownList>(Page, ref dropdownControls);
            foreach (var dropdownControl in dropdownControls)
            {
                if (dropdownControl.ID == "selItems")
                {
                    dropdownControl.SelectedValue = key;
                    ((IPostBackDataHandler)dropdownControl).RaisePostDataChangedEvent();

                }
            }


        }
        /// <summary>
        /// Add Dictionary Item to list
        /// </summary>
        /// <param name="item"></param>
        /// <param name="list"></param>
        private void AddToList(Dictionary.DictionaryItem item, ref List<Dictionary.DictionaryItem> list)
        {
            list.Add(item);
            if (!item.hasChildren)
            {
                return;
            }
            foreach (Dictionary.DictionaryItem child in item.Children)
            {
                AddToList(child, ref list);
            }
        }
        /// <summary>
        /// Get the value for the dictionary key for the listview
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetValue(string key)
        {
            if (Dictionary.DictionaryItem.hasKey(key))
                return new Dictionary.DictionaryItem(key).Value(SelectedLanguage);
            else
                return "";
        }
        /// <summary>
        /// Do the search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            {
                // clear DictionaryDashboard settings
                SetDictionaryKey(String.Empty);
                // build All Dictionary Items list
                PopulateAllDictionaryItems();
                // hide search results
                lvSearchResults.Visible = false;
             // get selected language code
                var languageCode = !String.IsNullOrWhiteSpace(ddlLanguage.SelectedValue) ? Int32.Parse(ddlLanguage.SelectedValue) : 1;

                if (!String.IsNullOrWhiteSpace(txtSearchTerm.Text))
                {
                    // need to loop through all dictionary items and find any matching the search text
                    foreach (var item in AllDictionaryItems)
                    {
                        string dicValue = item.Value(languageCode);
                        if (dicValue.ToLower() == txtSearchTerm.Text.Trim().ToLower())
                        {
                            // exact match
                            MatchingItems.Add(item);
                        }
                        else // only search for near matches if there are no exact matches is that the right logic ?
                        {
                            if (dicValue.ToLower().Contains(txtSearchTerm.Text.Trim().ToLower()))
                            {
                                // contains
                                ContainingItems.Add(item);

                            }
                        }
                    }
                    if (MatchingItems.Any()) //bind any matches
                    {
                        lvSearchResults.DataSource = MatchingItems;
                        lvSearchResults.DataBind();
                        lvSearchResults.Visible = true;
                    }
                    else
                    {
                        if (ContainingItems.Any()) //bind any near matches
                        {
                            lvSearchResults.DataSource = ContainingItems;
                            lvSearchResults.DataBind();
                            lvSearchResults.Visible = true;
                        }

                        else
                        {
                            // show helpful message
                            litStatusMessage.Text = "No Matching items";
                            litStatusMessage.Visible = true;
                        }
                    }


                }


            }
        }

        /// <summary>
        /// list view had linkbutton with command argument, to trigger setting of the DictionaryDashboard entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkButton_OnCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "editdicitem")
            {
                SetDictionaryKey(e.CommandArgument.ToString());

            }
        }
        /// <summary>
        /// Recursive helper to find the dropdown of the other usercontrol
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="list"></param>
        protected void FindChildControlsRecursive<T>(Control control, ref List<T> list)
        {

            foreach (var childControl in control.Controls)
            {
                if (childControl.GetType() == typeof(T))
                {
                    list.Add((T)childControl);
                }
                else
                {
                    FindChildControlsRecursive<T>((Control)childControl, ref list);
                }
            }

        }

    }
}