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
namespace GPUMauiLib.GPUViews
{
	public interface ITouchGPUView : IGPUView
	{
        /// <summary>
        /// Raised when a pointer enters the hit test area of the GraphicsView.
        /// </summary>
        /// <param name="points">The set of positions where there has been interaction.</param>
        void StartHoverInteraction(PointF[] points);

        /// <summary>
        /// Raised when a pointer moves while the pointer remains within the hit test 
        /// area of the GraphicsView.
        /// </summary>
        /// <param name="points">The set of positions where there has been interaction.</param>
        void MoveHoverInteraction(PointF[] points);

        /// <summary>
        /// Raised when a pointer leaves the hit test area of the GraphicsView.
        /// </summary>
        void EndHoverInteraction();

        /// <summary>
        /// Raised when the GraphicsView is pressed.
        /// </summary>
        /// <param name="points">The set of positions where there has been interaction.</param>
        void StartInteraction(PointF[] points);

        /// <summary>
        /// Raised when the GraphicsView is dragged.
        /// </summary>
        /// <param name="points">The set of positions where there has been interaction.</param>
        void DragInteraction(PointF[] points);

        /// <summary>
        /// Raised when the press that raised the StartInteraction event is released.
        /// </summary>
        /// <param name="points">The set of positions where there has been interaction.</param>
        /// <param name="isInsideBounds">a boolean that indicates if the interaction takes place within the bounds.</param>
        void EndInteraction(PointF[] points, bool isInsideBounds);

        /// <summary>
        /// Raised when the press that made contact with the GraphicsView loses contact.
        /// </summary>
        void CancelInteraction();

    }
}

