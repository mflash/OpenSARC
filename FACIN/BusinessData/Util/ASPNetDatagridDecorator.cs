using System;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
namespace BusinessData.Util
{
	/// <summary>
	/// Summary description for ASPNetDatagridDecorator.
	/// </summary>
	public class ASPNetDatagridDecorator
	{
		public ASPNetDatagridDecorator()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		private ASPNetDatagridDecorator(DataGrid DatagridToDecorate, ArrayList HeaderCells)
		{
			//
			// TODO: Add constructor logic here
			//
			this.DatagridToDecorate = DatagridToDecorate;
			AddMergeHeader(HeaderCells);
		}
		public void AddMergeHeader(ArrayList arrHeaderCells)
		{
			m_arrHeaderCells =arrHeaderCells;
		}
		private void NewRenderMethod(HtmlTextWriter writer, Control ctl)
		{			
			int iCurrIndex = 0;
			for(int i=0; i<m_arrHeaderCells.Count; i++)
			{
				TableCell item = (TableCell)m_arrHeaderCells[i];				
				if(item.ColumnSpan > 1)
				{
					iCurrIndex += item.ColumnSpan-1;
				}
				if(item.RowSpan > 1)
				{
					m_htblRowspanIndex.Add(iCurrIndex + i, iCurrIndex + i);
				}
				item.RenderControl(writer);
			}
			writer.WriteEndTag("TR");			
			//*** Add the style attributes that was defined in design time
			//	  to our second row so they both will have the same appearance
			m_dgDatagridToDecorate.HeaderStyle.AddAttributesToRender(writer);			
			//*** Insert the second row
			writer.RenderBeginTag("TR");
			//*** Render all the cells that was defined in design time, except the last one
			//	  because we already rendered it above
			for(int i=0; i< ctl.Controls.Count; i++)
			{
				if((null == m_htblRowspanIndex[i]))
				{
					ctl.Controls[i].RenderControl(writer);
				}
			}			
			//*** We don't need to write the </TR> close tag because the writer will do that for us
			//	  and so we're done :)
		}
		/// <summary>
		/// Gets or sets the datagrid to decorate
		/// </summary>
		public DataGrid DatagridToDecorate
		{
			get
			{
				return m_dgDatagridToDecorate;
			}
			set
			{
				if(null != m_dgDatagridToDecorate)
				{
					m_dgDatagridToDecorate.ItemCreated -= new DataGridItemEventHandler(DatagridToDecorate_ItemCreated);
				}
				m_dgDatagridToDecorate = value;	
				m_dgDatagridToDecorate.ItemCreated += new DataGridItemEventHandler(DatagridToDecorate_ItemCreated);
			}
		}
		private void DatagridToDecorate_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			//*** Examine if the item created is the header item
			ListItemType lit = e.Item.ItemType;
			if(ListItemType.Header == lit)
			{
				//*** Redirect the default header rendering method to our own method
				e.Item.SetRenderMethodDelegate(new RenderMethod(NewRenderMethod));				
			}
		}

		/// <summary>
		/// Hold the reference to the datagrid to decorate
		/// </summary>
		private DataGrid m_dgDatagridToDecorate = null;
		private ArrayList m_arrHeaderCells = null;
		private Hashtable m_htblRowspanIndex = new Hashtable();

	}
}
