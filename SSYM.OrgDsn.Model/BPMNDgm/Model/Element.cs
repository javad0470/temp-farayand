using SSYM.OrgDsn.Model.Enum;
using System.Collections.Generic;
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
using yWorks.Support.Annotations;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.Model.BPMNDgm.Model
{
    /// <summary>
    /// Abstract base class for all element types.
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// Check if the element accepts the outgoing edge.
        /// </summary>
        /// <param name="inSamePool">Are the nodes in the same pool?</param>
        /// <param name="relation">The relation.</param>
        /// <param name="other">The target element.</param>
        /// <returns>
        ///   <c>true</c> if the relation is acceptable; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool AcceptOutgoingEdge(bool? inSamePool, Relation relation, [CanBeNull] Element other)
        {
            // don't allow relations of type message flow
            return relation.Type != RelationType.MessageFlow
                // don't allow relations between different pools (unless other element is unknown)
                // and the target accepts this kind of relation
              && (other == null || inSamePool == true && other.AcceptIncomingRelation(relation));
        }

        /// <summary>
        /// Check if the incoming relation is acceptable.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <returns>
        ///   <c>true</c> if the relation is acceptable; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool AcceptIncomingRelation(Relation relation)
        {
            return relation.Type != RelationType.MessageFlow;
        }

        public IRow Col { get; set; }
    }

    #region Activity

    /// <summary>
    /// Concrete implementation of <see cref="Element"/> that models a BPMN activity.
    /// </summary>
    public class Activity : Element
    {
        public Activity() : this(TypShp.A1) { }

        public Activity(TypShp type)
        {
            Type = type;
            AttachedNodes = new List<INode>();
        }

        /// <summary>
        /// 
        /// </summary>
        public yWorks.Canvas.Geometry.Structs.SizeD Siz
        {
            get
            {
                return new yWorks.Canvas.Geometry.Structs.SizeD(100, 50);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<INode> AttachedNodes { get; set; }

        /// <summary>
        /// child table embeded in activity for attaching boundary nodes
        /// </summary>
        public INode TblChld { get; set; }

        /// <summary>
        /// Gets or sets the actual activity type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public TypShp Type { get; set; }

        public override bool AcceptOutgoingEdge(bool? inSamePool, Relation relation, [CanBeNull] Element other)
        {
            // message flows are only allowed between nodes of type activity or message event
            if (relation.Type == RelationType.MessageFlow)
            {
                return other == null || (other.AcceptIncomingRelation(relation) && inSamePool == false);
            }
            return base.AcceptOutgoingEdge(inSamePool, relation, other);
        }

        public override bool AcceptIncomingRelation(Relation relation)
        {
            // allows all types of relations (including message flows)
            return true;
        }

        /// <summary>
        /// Determines whether this instance is a subprocess.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is a subprocess; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSubprocess()
        {
            switch (Type)
            {
                case TypShp.CA1:
                case TypShp.CA2:
                case TypShp.CA3:
                case TypShp.CA4:
                case TypShp.CA5:
                case TypShp.CA6:
                    return true;
                default:
                    return false;
            }
        }
    }

    /// <summary>
    /// Contains all supported activity types.
    /// </summary>
    //public enum ActivityType
    //{
    //    A1,//Abstract Task
    //    A2,//Receive Task
    //    A3,//Receive Task that Instantiate  a Process
    //    A4,//Service Task
    //    A5,//Send Task
    //    A6,//User Task
    //    A7,//Manual Task
    //    A8,//Business Rule Task
    //    A9,//Script Task
    //    CA1,//Call Activity Calling a Global Task
    //    CA2,//Call Activity Calling a User Global Task
    //    CA3,//Call Activity Calling a Manual Global Task
    //    CA4,//Call Activity Calling a Business Rule Global Task
    //    CA5,//Call Activity Calling a Script Global Task
    //    CA6,//Call Activity Calling a Process

    //  Task, TaskMultipleInstances, TaskLoop, TaskAdHoc,
    //  Subprocess, SubprocessMultipleInstances, SubprocessLoop, SubprocessAdHoc
    //}

    #endregion

    #region Gateway

    /// <summary>
    /// Concrete implementation of <see cref="Element"/> that models a BPMN gateway.
    /// </summary>
    public class Gateway : Element
    {
        public TypShp Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public yWorks.Canvas.Geometry.Structs.SizeD Siz
        {
            get
            {
                return new yWorks.Canvas.Geometry.Structs.SizeD(50, 50);
            }
        }
    }

    /// <summary>
    /// Contains all supported gateway types
    /// </summary>
    //public enum GatewayType
    //{
    //    G1,//Exclusive Gateway
    //    G2,//Inclusive Gateway
    //    G3,//Parallel Gateway
    //    G4,//Complex Gateway
    //    G5,//Intermediate Exclusive Event Based Gateway
    //    G6,//Start Exclusive Event Based Gateway
    //    G7,//Intermediate Parallel Event Based Gateway
    //    G8,//Start Parallel Event Based Gateway

    //  DataBasedExclusive, EventBasedExclusive, Parallel, Inclusive, Complex
    //}

    #endregion

    #region Event

    /// <summary>
    /// Concrete implementation of <see cref="Element"/> that models a BPMN event.
    /// </summary>
    public class Event : Element
    {
        public override bool AcceptIncomingRelation(Relation relation)
        {
            // always disallow incoming relations if we are a start event
            if (IsStart())
            {
                return false;
            }
            // allow incoming relations of type message flow only if we're a message event
            if (relation.Type == RelationType.MessageFlow && IsMessageEvent())
            {
                return true;
            }
            return base.AcceptIncomingRelation(relation);
        }

        public override bool AcceptOutgoingEdge(bool? inSamePool, Relation relation, [CanBeNull] Element other)
        {
            // end events can't have outgoing edges
            if (IsEnd())
            {
                return false;
            }
            // message flows are only allowed between nodes of type activity or message event
            if (relation.Type == RelationType.MessageFlow)
            {
                return other == null || (other.AcceptIncomingRelation(relation) && inSamePool == false);
            }
            return base.AcceptOutgoingEdge(inSamePool, relation, other);
        }

        /// <summary>
        /// Gets or sets the type of this event.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public TypShp Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public yWorks.Canvas.Geometry.Structs.SizeD Siz
        {
            get
            {
                return new yWorks.Canvas.Geometry.Structs.SizeD(25, 25);
            }
        }

        /// <summary>
        /// Determines whether this instance is a message event.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is a message event; otherwise, <c>false</c>.
        /// </returns>
        private bool IsMessageEvent()
        {
            return Type == TypShp.E13 || Type == TypShp.E14
                 || Type == TypShp.E15 || Type == TypShp.E16
                 || Type == TypShp.E17 || Type == TypShp.E18
                 || Type == TypShp.E19 || Type == TypShp.E20;
        }

        /// <summary>
        /// Determines whether this instance is a terminal event.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is terminal; otherwise, <c>false</c>.
        /// </returns>
        private bool IsEnd()
        {
            switch (Type)
            {
                case TypShp.E20:
                case TypShp.E23:
                case TypShp.E25:
                case TypShp.E33:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether this instance is a start event.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is a start event; otherwise, <c>false</c>.
        /// </returns>
        private bool IsStart()
        {
            switch (Type)
            {
                case TypShp.E1:
                case TypShp.E2:
                case TypShp.E3:
                case TypShp.E7:
                case TypShp.E8:
                case TypShp.E9:
                case TypShp.E13:
                case TypShp.E14:
                case TypShp.E15:
                case TypShp.E21:
                case TypShp.E26:
                case TypShp.E27:
                case TypShp.E28:
                    return true;
                default:
                    return false;
            }
        }
    }

    /// <summary>
    /// Contains all supported event types.
    /// </summary>
    //public enum EventType
    //{
    //    E1,//Top-Level Start Timer  Event 
    //    E2,// Sub-Process Interrupting Start Timer Event
    //    E3,// Sub-Process Non-Interrupting Start Timer Event
    //    E4,// Catching Intermediate Timer Event
    //    E5,// Boundary Interrupting Timer Event
    //    E6,// Boundary Non-Interrupting Timer Event
    //    E7,//Top-Level Start Conditional Event 
    //    E8,// Sub-Process Interrupting Start Conditional Event
    //    E9,// Sub-Process Non-Interrupting Start Conditional Event
    //    E10,// Catching Intermediate Conditional Event
    //    E11,// Boundary Interrupting Conditional Event
    //    E12,// Boundary Non-Interrupting Conditional Event
    //    E13,//Top-Level Start Message Event 
    //    E14,// Sub-Process Interrupting Start Message Event
    //    E15,// Sub-Process Non-Interrupting Start Message Event
    //    E16,// Catching Intermediate Message Event
    //    E17,// Boundary Interrupting Message Event
    //    E18,// Boundary Non-Interrupting Message Event
    //    E19,// Throwing Intermediate Message Event
    //    E20,//End Message Event
    //    E21,// Sub-Process Interrupting Start Error Event
    //    E22,//Boundary Interrupting Error Event
    //    E23,//End Error Event
    //    E24,//Boundary Interrupting Cancel Event
    //    E25,//End Cancel Event
    //    E26,//Top-Level Start Signal Event 
    //    E27,// Sub-Process Interrupting Start Signal Event
    //    E28,// Sub-Process Non-Interrupting Start Signal Event
    //    E29,// Catching Intermediate Signal Event
    //    E30,// Boundary Interrupting Signal Event
    //    E31,// Boundary Non-Interrupting Signal Event
    //    E32,// Throwing Intermediate Signal Event
    //    E33,//End Signal Event

    //  PlainStart, PlainIntermediateCatching, PlainEnd,
    //  MessageStart, MessageIntermediateCatching, MessageIntermediateThrowing, MessageEnd,
    //  TimerStart, TimerIntermediateCatching,
    //  Terminate,
    //  MultipleStart, MultipleIntermediateCatching, MultipleIntermediateThrowing, MultipleEnd,
    //  ErrorIntermediateCatching, ErrorEnd
    //}

    #endregion

    #region Artifact

    /// <summary>
    /// Concrete implementation of <see cref="Element"/> that models a BPMN artifact.
    /// </summary>
    public class Artifact : Element
    {
        public TypShp Type { get; set; }

        public override bool AcceptOutgoingEdge(bool? inSamePool, Relation relation, [CanBeNull] Element other)
        {
            // only allow relations that are associations
            return relation.IsAssociation() && (other == null || other.AcceptIncomingRelation(relation));
        }

        public override bool AcceptIncomingRelation(Relation relation)
        {
            // only allow relations that are associations
            return relation.IsAssociation();
        }

        /// <summary>
        /// 
        /// </summary>
        public yWorks.Canvas.Geometry.Structs.SizeD Siz
        {
            get
            {
                if (this.Type == TypShp.DO1)
                {
                    return new yWorks.Canvas.Geometry.Structs.SizeD(30, 50);
                }

                else if (this.Type == TypShp.DO2)
                {
                    return new yWorks.Canvas.Geometry.Structs.SizeD(20, 10);
                }

                return new yWorks.Canvas.Geometry.Structs.SizeD(30, 50);
            }
        }
    }

    /// <summary>
    /// Contains all supported artifact types.
    /// </summary>
    //public enum ArtifactType
    //{
    //    DO1,//Data Object
    //    DO2,//Message

    //  DataObject, TextAnnotation
    //}

    #endregion

    #region Relation

    /// <summary>
    /// Contains all supported relation types.
    /// </summary>
    public enum RelationType
    {
        SequenceFlow, DefaultFlow, ConditionalFlow, Association, DirectedAssociation, BiDirectedAssociation, MessageFlow, NonDirectedMessageFlow
    }

    /// <summary>
    /// Models a BPMN relation.
    /// </summary>
    public class Relation
    {
        /// <summary>
        /// Gets or sets the type of this relation.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public RelationType Type { get; set; }

        /// <summary>
        /// Determines whether this instance is an association.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is an association; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAssociation()
        {
            return Type == RelationType.Association || Type == RelationType.DirectedAssociation ||
                   Type == RelationType.BiDirectedAssociation;
        }
    }

    #endregion
}
