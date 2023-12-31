// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2023 ILGPU Project
//                                    www.ilgpu.net
//
// File: .cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace VulkanPlatform
{

#if DEBUG

	public static class VulkanFlowTracer
	{
		public static List<FlowTracerItem> WorkFlow = new List<FlowTracerItem>();

		static int icnt = 0;
		static bool logging = true;
		public static bool Logging { get { return logging; } set { logging = value; } }

		public static void AddItem(string action)
		{
			if (logging)
			{
				icnt++;

				FlowTracerItem item = new FlowTracerItem() { Step = icnt, Action = action };

				WorkFlow.Add(item);
			}
		}

		public static string FlowContentLog()
		{
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < VulkanFlowTracer.WorkFlow.Count; i++)
            {
                FlowTracerItem item = VulkanFlowTracer.WorkFlow[i];
                sb.Append(item.Step.ToString() + " " + item.Action + System.Environment.NewLine);
            }
            return sb.ToString();
        }
	}

	public struct FlowTracerItem
	{
		public int Step;
		public string Action;
	}
#endif
}

