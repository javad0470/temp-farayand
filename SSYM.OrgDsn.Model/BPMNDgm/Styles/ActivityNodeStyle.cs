/****************************************************************************
 **
 ** This file is part of yFiles WPF 2.4.
 ** 
 ** yWorks proprietary/confidential. Use is subject to license terms.
 **
 ** Redistribution of this file or of an unauthorized byte-code version
 ** of this file is strictly forbidden.
 **
 ** Copyright (c) 2003-2013 by yWorks GmbH, Vor dem Kreuzberg 28, 
 ** 72070 Tuebingen, Germany. All rights reserved.
 **
 ***************************************************************************/
using System;
using System.Windows;
using System.Windows.Shapes;
using yWorks.Canvas.Drawing;
using yWorks.Canvas.Geometry.Structs;
using yWorks.yFiles.UI.Drawing;
using yWorks.yFiles.UI.LabelModels;
using yWorks.yFiles.UI.Model;
using yWorks.Support.Extensions;
using SSYM.OrgDsn.Model.BPMNDgm.Model;
using SSYM.OrgDsn.Model.Enum;
using yWorks.yFiles.Layout.Hierarchic.Incremental;

namespace SSYM.OrgDsn.Model.BPMNDgm.Styles
{
    /// <summary>
    /// Concrete implementation of <see cref="BPDMNodeStyleBase"/> for Activity node types.
    /// </summary>
    /// <remarks>
    /// In addition to the lookups provided by its parent classes, this implementation
    /// will also provide an <see cref="IInsetsProvider{INode}"/>.
    /// </remarks>
    public class ActivityNodeStyle : BPDMNodeStyleBase, IStyle
    {

        #region Properties

        public IRow Col
        {
            get
            {
                return this.Activity.Col;
            }
            set
            {
                this.Activity.Col = value;
            }
        }

        /// <summary>
        /// The outline of the node
        /// </summary>
        private static readonly Rectangle Outline = new Rectangle { RadiusX = 5, RadiusY = 5 };

        /// <summary>
        /// The insets used by the <see cref="IInsetsProvider{INode}"/>
        /// </summary>
        private InsetsD insets = new InsetsD(10, 10, 10, 10);

        /// <summary>
        /// Backing field for the <see cref="Activity"/> property.
        /// </summary>
        private Activity activity;

        /// <summary>
        /// Gets or sets the activity.
        /// </summary>
        /// <remarks>
        /// Depending on the type of the activity the insets of the node will be modified.
        /// </remarks>
        /// <value>
        /// The activity.
        /// </value>
        public Activity Activity
        {
            get { return activity; }
            set
            {
                activity = value;
                // subprocesses need bigger insets so that they can place their label without collision with their content
                insets = activity.IsSubprocess() ? new InsetsD(10, 25, 10, 10) : new InsetsD(2, 2, 2, 3);
            }
        }

        public ActivityNodeStyle()
            : this(new Activity(TypShp.A1))
        {
        }

        public ActivityNodeStyle(Activity type)
        {
            Activity = type;
        }

        /// <summary>
        /// Gets the resource key for the XAML lookup.
        /// </summary>
        /// <remarks>
        /// This key will be of type <see cref="ActivityType"/>.
        /// </remarks>
        protected override object ResourceKey
        {
            get { return Activity.Type; }
        }

        protected override Element Element
        {
            get { return Activity; }
        }

        #endregion

        protected override object Lookup(INode node, Type type)
        {
            // add the IInsetsProvider<INode>
            if (type == typeof(IInsetsProvider<INode>))
            {
                return new MyInsetProvider(this);
            }
            return base.Lookup(node, type);
        }

        protected override GeneralPath GetOutline(INode node)
        {
            return GeneralPath.CreateGeneralPathFromShape(Outline, node.Layout.ToRectD());
        }

        /// <summary>
        /// Creates the default label configuration.
        /// </summary>
        /// <remarks>
        /// Activities that are subprocesses will place the label at the upper left corner of the node.
        /// </remarks>
        /// <param name="labeledItem">The labeled item.</param>
        /// <param name="param">The param.</param>
        /// <param name="style">The style.</param>
        /// <param name="preferredSize">Size of the preferred.</param>
        public override void CreateDefaultLabelConfiguration(ILabeledItem labeledItem, out ILabelModelParameter param,
                                                               out ILabelStyle style, out SizeD? preferredSize)
        {
            if (Activity.IsSubprocess())
            {
                var model = new InteriorLabelModel { Insets = new InsetsD(10, 10, 10, 10) };
                param = model.CreateParameter(InteriorLabelModel.Position.NorthWest);
                style = new SimpleLabelStyle
                {
                    TypefaceFormat = { TextAlignment = TextAlignment.Center }
                };
                preferredSize = null;
            }
            else
            {
                base.CreateDefaultLabelConfiguration(labeledItem, out param, out style, out preferredSize);
            }
        }

        /// <summary>
        /// Delegates its logic to the node style.
        /// </summary>
        private class MyInsetProvider : IInsetsProvider<INode>
        {
            private readonly ActivityNodeStyle style;

            internal MyInsetProvider(ActivityNodeStyle style)
            {
                this.style = style;
            }

            public InsetsD GetInsets(INode item)
            {
                return style.insets;
            }
        }


    }
}
