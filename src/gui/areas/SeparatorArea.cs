// created on 6/18/2004 at 7:46 PM
/*
 *   Copyright (c) 2004, Alexandros Frantzis (alf82 [at] freemail [dot] gr)
 *
 *   This file is part of Bless.
 *
 *   Bless is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 *   Bless is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with Bless; if not, write to the Free Software
 *   Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
using System;
using Gtk;
using Gdk;
using Bless.Gui.Drawers;
using Bless.Util;

namespace Bless.Gui.Areas {

///<summary>An area that contains a vertical separator line</summary>
public class SeparatorArea : Area {
	
	Gdk.GC lineGC;
	
	public SeparatorArea()
	: base() 
	{
		type="separator";
	}
	
	public override void Realize(Gtk.DrawingArea da, Drawer d)
	{
		base.Realize(da, d);
		
		lineGC=new Gdk.GC(da.GdkWindow);
		
		lineGC.RgbFgColor=drawer.Info.fgNormal[(int)Drawer.RowType.Even, (int)Drawer.ColumnType.Even];
	}
	
	protected override void RenderRange(Bless.Util.Range range, Drawer.HighlightType ht)
	{
	}
	
	protected override void RenderRowNormal(int i, int p, int n, bool blank)
	{
	}
	
	protected override void RenderRowHighlight(int i, int p, int n, bool blank, Drawer.HighlightType ht)
	{
	}
	
	public override void Scroll(long offset)
	{
		if (isAreaRealized==false)
			return;
		
		int nrows=height/drawer.Height;
		long bleft=nrows*bpr;
		int rfull=0;
		int blast=0;
		
		if (bpr>0) {
			if (bleft+offset > byteBuffer.Size)
				bleft=byteBuffer.Size - offset + 1;
		
			// calculate number of full rows
			// and number of bytes in last (non-full)
			rfull=(int)(bleft/bpr);
			blast=(int)(bleft%bpr);
		
			if (blast!=0)
				rfull++;
		}
		
		if (rfull==0)
			return;
		
		this.offset=offset;
		
		// draw seperator 
		backPixmap.DrawLine(lineGC, x+drawer.Width/2, 0, x+drawer.Width/2, drawer.Height*rfull);
	
	}

	public override int CalcWidth(int n, bool force) 
	{
		return drawer.Width; 
	}
	
	public override void GetDisplayInfoByOffset(long off, out int orow, out int obyte, out int ox, out int oy)
	{	 
		orow=(int)((off-offset)/bpr);
		obyte=(int)((off-offset)%bpr);
			
		oy=orow*drawer.Height;
		
		ox=0;
	} 

	public override long GetOffsetByDisplayInfo(int x, int y, out int digit, out  GetOffsetFlags flags)
	{
		flags=0;
		int row=y/drawer.Height;
		long off=row*bpr+offset;
		if (off >= byteBuffer.Size)
			flags |= GetOffsetFlags.Eof;
			
		digit=0;
	
		return off;
	}
	
	public override void SetSelection(long start, long end)
	{
		SetSelectionNoRender(start, end);
	}
	
	public override void MoveCursor(long offset, int digit)
	{
		MoveCursorNoRender(offset, digit);
	}
}


}//namespace