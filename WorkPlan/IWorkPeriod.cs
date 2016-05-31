﻿using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace WorkPlan
{
    public interface IWorkPeriod
    {
        int Id { get; set; }
        Employee Employee { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        bool FullDay { get; }
        string Notes { get; set; }
        void Draw(DataGridViewCellPaintingEventArgs e, int order = 0);
        void Print(PrintPageEventArgs e, Rectangle cell, int totalPerDay, int order = 0);
        bool IsAfternoon { get; }
    }
}
