using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.Utility
{
    public partial class XPDLGenerator
    {
        //Templates
        string DiagramTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                 "<Package xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Id=\"{0}\" Name=\"{1}\" xmlns=\"http://www.wfmc.org/2009/XPDL2.2\">" +
                                 "<PackageHeader>" +
                                 "<XPDLVersion>2.2</XPDLVersion>" +
                                 "<Vendor>BizAgi Process Modeler.</Vendor>" +
                                 "<Created>{2}</Created>" +
                                 "<Description>{1}</Description>" +
                                 "<Documentation />" +
                                 "</PackageHeader>" +
                                 "<RedefinableHeader>" +
                                 "<Author>SSYM</Author>" +
                                 "<Version>1.0</Version>" +
                                 "<Countrykey>CO</Countrykey>" +
                                 "</RedefinableHeader>" +
                                 "<ExternalPackages />" +
                                 "<Pools/>" +
                                 "<MessageFlows />" +
                                 "<Associations />" +
                                 "<Artifacts />" +
                                 "<WorkflowProcesses>" +
                                 "(PROCES)" +
                                 "</WorkflowProcesses>" +
                                 "<ExtendedAttributes />" +
                                 "</Package>";

        private string WorkflowProcessTemplate = "<WorkflowProcess Id=\"{0}\" Name=\"{1}\">" +
                                                 "<ProcessHeader>" +
                                                 "<Created>{2}</Created>" +
                                                 "<Description />" +
                                                 "</ProcessHeader>" +
                                                 "<RedefinableHeader>" +
                                                 "<Author />" +
                                                 "<Version />" +
                                                 "<Countrykey>CO</Countrykey>" +
                                                 "</RedefinableHeader>" +
                                                 "<ActivitySets />" +
                                                 "<DataInputOutputs />" +
                                                 "<Activities/>" +
                                                 "<DataObjects/>" +
                                                 "<Transitions/>" +
                                                 "<DataAssociations/>" +
                                                 "<ExtendedAttributes />" +
                                                 "</WorkflowProcess>";

        string ActivityTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                  "<Description />" +
                                  "<Implementation>" +
                                  " <Task />" +
                                  "</Implementation>" +
                                  "<Performers />" +
                                  "<Documentation />" +
                                  "<InputSets />" +
                                  "<OutputSets />" +
                                  "<Loop LoopType=\"None\" />" +
                                  "<NodeGraphicsInfos>" +
                                  " <NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-16553830\" FillColor=\"-1249281\">" +
                                  "  <Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                  "</NodeGraphicsInfo>" +
                                  "</NodeGraphicsInfos>" +
                                  "<ExtendedAttributes />" +
                                  "</Activity>";

        private string CallingActivityTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                                 "<Description />" +
                                                 "<Implementation>" +
                                                 " <SubFlow />" +
                                                 "</Implementation>" +
                                                 "<Performers />" +
                                                 "<Documentation />" +
                                                 "<InputSets />" +
                                                 "<OutputSets />" +
                                                 "<Loop LoopType=\"None\" />" +
                                                 "<NodeGraphicsInfos>" +
                                                 " <NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-16553830\" FillColor=\"-1249281\">" +
                                                 "  <Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                 "</NodeGraphicsInfo>" +
                                                 "</NodeGraphicsInfos>" +
                                                 "<ExtendedAttributes />" +
                                                 "</Activity>";
        string ActivityReceiveTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                         "<Description />" +
                                         "<Implementation>" +
                                         "<Task>" +
                                         "<TaskReceive Instantiate=\"false\" />" +
                                         "</Task>" +
                                         "</Implementation>" +
                                         "<Performers />" +
                                         "<Documentation />" +
                                         "<InputSets />" +
                                         "<OutputSets />" +
                                         "<Loop LoopType=\"None\" />" +
                                         "<NodeGraphicsInfos>" +
                                         " <NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-16553830\" FillColor=\"-1249281\">" +
                                         "  <Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                         "</NodeGraphicsInfo>" +
                                         "</NodeGraphicsInfos>" +
                                         "<ExtendedAttributes />" +
                                         "</Activity>";

        private string ActivityReceiveInstantiateTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                                            "<Description />" +
                                                            "<Implementation>" +
                                                            "<Task>" +
                                                            "<TaskReceive Instantiate=\"false\" />" +
                                                            "</Task>" +
                                                            "</Implementation>" +
                                                            "<Performers />" +
                                                            "<Documentation />" +
                                                            "<InputSets />" +
                                                            "<OutputSets />" +
                                                            "<Loop LoopType=\"None\" />" +
                                                            "<NodeGraphicsInfos>" +
                                                            " <NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-16553830\" FillColor=\"-1249281\">" +
                                                            "  <Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                            "</NodeGraphicsInfo>" +
                                                            "</NodeGraphicsInfos>" +
                                                            "<ExtendedAttributes />" +
                                                            "</Activity>";
        string ActivityServiceTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                         "<Description />" +
                                         "<Implementation>" +
                                         "<Task>" +
                                         "<TaskService/>" +
                                         "</Task>" +
                                         "</Implementation>" +
                                         "<Performers />" +
                                         "<Documentation />" +
                                         "<InputSets />" +
                                         "<OutputSets />" +
                                         "<Loop LoopType=\"None\" />" +
                                         "<NodeGraphicsInfos>" +
                                         " <NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-16553830\" FillColor=\"-1249281\">" +
                                         "  <Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                         "</NodeGraphicsInfo>" +
                                         "</NodeGraphicsInfos>" +
                                         "<ExtendedAttributes />" +
                                         "</Activity>";
        string ActivitySendTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                         "<Description />" +
                                         "<Implementation>" +
                                         "<Task>" +
                                         "<TaskSend/>" +
                                         "</Task>" +
                                         "</Implementation>" +
                                         "<Performers />" +
                                         "<Documentation />" +
                                         "<InputSets />" +
                                         "<OutputSets />" +
                                         "<Loop LoopType=\"None\" />" +
                                         "<NodeGraphicsInfos>" +
                                         " <NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-16553830\" FillColor=\"-1249281\">" +
                                         "  <Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                         "</NodeGraphicsInfo>" +
                                         "</NodeGraphicsInfos>" +
                                         "<ExtendedAttributes />" +
                                         "</Activity>";
        string ActivityUserTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                         "<Description />" +
                                         "<Implementation>" +
                                         "<Task>" +
                                         "<TaskUser Implementation=\"Unspecified\" />" +
                                         "</Task>" +
                                         "</Implementation>" +
                                         "<Performers />" +
                                         "<Documentation />" +
                                         "<InputSets />" +
                                         "<OutputSets />" +
                                         "<Loop LoopType=\"None\" />" +
                                         "<NodeGraphicsInfos>" +
                                         " <NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-16553830\" FillColor=\"-1249281\">" +
                                         "  <Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                         "</NodeGraphicsInfo>" +
                                         "</NodeGraphicsInfos>" +
                                         "<ExtendedAttributes />" +
                                         "</Activity>";
        string ActivityManualTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                         "<Description />" +
                                         "<Implementation>" +
                                         "<Task>" +
                                         "<TaskManual/>" +
                                         "</Task>" +
                                         "</Implementation>" +
                                         "<Performers />" +
                                         "<Documentation />" +
                                         "<InputSets />" +
                                         "<OutputSets />" +
                                         "<Loop LoopType=\"None\" />" +
                                         "<NodeGraphicsInfos>" +
                                         " <NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-16553830\" FillColor=\"-1249281\">" +
                                         "  <Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                         "</NodeGraphicsInfo>" +
                                         "</NodeGraphicsInfos>" +
                                         "<ExtendedAttributes />" +
                                         "</Activity>";
        string ActivityBusinessRuleTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                              "<Description />" +
                                              "<Implementation>" +
                                              "<Task>" +
                                              "<TaskBusinessRule BusinessRuleTaskImplementation=\"Unspecified\"/>" +
                                              "</Task>" +
                                              "</Implementation>" +
                                              "<Performers />" +
                                              "<Documentation />" +
                                              "<InputSets />" +
                                              "<OutputSets />" +
                                              "<Loop LoopType=\"None\" />" +
                                              "<NodeGraphicsInfos>" +
                                              " <NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-16553830\" FillColor=\"-1249281\">" +
                                              "  <Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                              "</NodeGraphicsInfo>" +
                                              "</NodeGraphicsInfos>" +
                                              "<ExtendedAttributes />" +
                                              "</Activity>";
        string ActivityScriptTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                         "<Description />" +
                                         "<Implementation>" +
                                         "<Task>" +
                                         "<Script/>" +
                                         "</Task>" +
                                         "</Implementation>" +
                                         "<Performers />" +
                                         "<Documentation />" +
                                         "<InputSets />" +
                                         "<OutputSets />" +
                                         "<Loop LoopType=\"None\" />" +
                                         "<NodeGraphicsInfos>" +
                                         " <NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-16553830\" FillColor=\"-1249281\">" +
                                         "  <Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                         "</NodeGraphicsInfo>" +
                                         "</NodeGraphicsInfos>" +
                                         "<ExtendedAttributes />" +
                                         "</Activity>";
        string EventSignalTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                     "  <Description />" +
                                     "<Event>" +
                                     "<StartEvent Trigger=\"Signal\">" +
                                     "<TriggerResultSignal />" +
                                     "</StartEvent>" +
                                     "</Event>" +
                                     "<Documentation />" +
                                     "<NodeGraphicsInfos>" +
                                     "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                     "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                     "</NodeGraphicsInfo>" +
                                     "</NodeGraphicsInfos>" +
                                     "<ExtendedAttributes />" +
                                     "</Activity>";
        string EventSignalNonTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                     "  <Description />" +
                                     "<Event>" +
                                     "<StartEvent Trigger=\"Signal\">" +
                                     "<TriggerResultSignal />" +
                                     "</StartEvent>" +
                                     "</Event>" +
                                     "<Documentation />" +
                                     "<NodeGraphicsInfos>" +
                                     "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                     "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                     "</NodeGraphicsInfo>" +
                                     "</NodeGraphicsInfos>" +
                                     "<ExtendedAttributes />" +
                                     "</Activity>";
        string EventTimerTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                    "  <Description />" +
                                    "<Event>" +
                                    "<StartEvent Trigger=\"Timer\">" +
                                    "<TriggerTimer />" +
                                    "</StartEvent>" +
                                    "</Event>" +
                                    "<Documentation />" +
                                    "<NodeGraphicsInfos>" +
                                    "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                    "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                    "</NodeGraphicsInfo>" +
                                    "</NodeGraphicsInfos>" +
                                    "<ExtendedAttributes />" +
                                    "</Activity>";
        string EventTimerNonTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                    "  <Description />" +
                                    "<Event>" +
                                    "<StartEvent Trigger=\"Timer\">" +
                                    "<TriggerTimer />" +
                                    "</StartEvent>" +
                                    "</Event>" +
                                    "<Documentation />" +
                                    "<NodeGraphicsInfos>" +
                                    "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                    "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                    "</NodeGraphicsInfo>" +
                                    "</NodeGraphicsInfos>" +
                                    "<ExtendedAttributes />" +
                                    "</Activity>";
        string EventErrorTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                    "  <Description />" +
                                    "<Event>" +
                                    "<StartEvent Trigger=\"None\">" +
                                    "</StartEvent>" +
                                    "</Event>" +
                                    "<Documentation />" +
                                    "<NodeGraphicsInfos>" +
                                    "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                    "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                    "</NodeGraphicsInfo>" +
                                    "</NodeGraphicsInfos>" +
                                    "<ExtendedAttributes />" +
                                    "</Activity>";
        string EventNoneTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                    "  <Description />" +
                                    "<Event>" +
                                    "<StartEvent Trigger=\"None\">" +
                                    "</StartEvent>" +
                                    "</Event>" +
                                    "<Documentation />" +
                                    "<NodeGraphicsInfos>" +
                                    "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                    "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                    "</NodeGraphicsInfo>" +
                                    "</NodeGraphicsInfos>" +
                                    "<ExtendedAttributes />" +
                                    "</Activity>";
        string EventMessageTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                      "  <Description />" +
                                      "<Event>" +
                                      "<StartEvent Trigger=\"Message\">" +
                                      "<TriggerResultMessage>" +
                                      "<Message Id=\"{6}\" />" +
                                      "</TriggerResultMessage>" +
                                      "</StartEvent>" +
                                      "</Event>" +
                                      "<Documentation />" +
                                      "<NodeGraphicsInfos>" +
                                      "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                      "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                      "</NodeGraphicsInfo>" +
                                      "</NodeGraphicsInfos>" +
                                      "<ExtendedAttributes />" +
                                      "</Activity>";
        string EventMessageNonTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                      "  <Description />" +
                                      "<Event>" +
                                      "<StartEvent Trigger=\"Message\">" +
                                      "<TriggerResultMessage>" +
                                      "<Message Id=\"{6}\" />" +
                                      "</TriggerResultMessage>" +
                                      "</StartEvent>" +
                                      "</Event>" +
                                      "<Documentation />" +
                                      "<NodeGraphicsInfos>" +
                                      "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                      "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                      "</NodeGraphicsInfo>" +
                                      "</NodeGraphicsInfos>" +
                                      "<ExtendedAttributes />" +
                                      "</Activity>";
        string EventIntemediateSignalTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                               "  <Description />" +
                                               "<Event>" +
                                               "<IntermediateEvent Trigger=\"Signal\">" +
                                               "<TriggerResultSignal />" +
                                               "</IntermediateEvent>" +
                                               "</Event>" +
                                               "<Documentation />" +
                                               "<NodeGraphicsInfos>" +
                                               "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                               "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                               "</NodeGraphicsInfo>" +
                                               "</NodeGraphicsInfos>" +
                                               "<ExtendedAttributes />" +
                                               "</Activity>";
        string EventIntemediateSignalNonTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                               "  <Description />" +
                                               "<Event>" +
                                               "<IntermediateEvent Trigger=\"Signal\">" +
                                               "<TriggerResultSignal />" +
                                               "</IntermediateEvent>" +
                                               "</Event>" +
                                               "<Documentation />" +
                                               "<NodeGraphicsInfos>" +
                                               "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                               "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                               "</NodeGraphicsInfo>" +
                                               "</NodeGraphicsInfos>" +
                                               "<ExtendedAttributes />" +
                                               "</Activity>";
        string EventThrowIntemediateSignalTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                               "  <Description />" +
                                               "<Event>" +
                                               "<IntermediateEvent Trigger=\"Signal\">" +
                                               "<TriggerResultSignal CatchThrow=\"THROW\"/>" +
                                               "</IntermediateEvent>" +
                                               "</Event>" +
                                               "<Documentation />" +
                                               "<NodeGraphicsInfos>" +
                                               "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                               "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                               "</NodeGraphicsInfo>" +
                                               "</NodeGraphicsInfos>" +
                                               "<ExtendedAttributes />" +
                                               "</Activity>";
        string EventIntemediateTimerTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                               "  <Description />" +
                                               "<Event>" +
                                               "<IntermediateEvent Trigger=\"Timer\">" +
                                               "<TriggerTimer />" +
                                               "</IntermediateEvent>" +
                                               "</Event>" +
                                               "<Documentation />" +
                                               "<NodeGraphicsInfos>" +
                                               "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                               "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                               "</NodeGraphicsInfo>" +
                                               "</NodeGraphicsInfos>" +
                                               "<ExtendedAttributes />" +
                                               "</Activity>";
        string EventIntemediateTimerNonTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                               "  <Description />" +
                                               "<Event>" +
                                               "<IntermediateEvent Trigger=\"Timer\">" +
                                               "<TriggerTimer />" +
                                               "</IntermediateEvent>" +
                                               "</Event>" +
                                               "<Documentation />" +
                                               "<NodeGraphicsInfos>" +
                                               "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                               "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                               "</NodeGraphicsInfo>" +
                                               "</NodeGraphicsInfos>" +
                                               "<ExtendedAttributes />" +
                                               "</Activity>";
        string EventIntemediateConditionalTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                                     "  <Description />" +
                                                     "<Event>" +
                                                     "<IntermediateEvent Trigger=\"Conditional\">" +
                                                     "<TriggerConditional />" +
                                                     "</IntermediateEvent>" +
                                                     "</Event>" +
                                                     "<Documentation />" +
                                                     "<NodeGraphicsInfos>" +
                                                     "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                     "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                     "</NodeGraphicsInfo>" +
                                                     "</NodeGraphicsInfos>" +
                                                     "<ExtendedAttributes />" +
                                                     "</Activity>";
        string EventIntemediateConditionalNonTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                                     "  <Description />" +
                                                     "<Event>" +
                                                     "<IntermediateEvent Trigger=\"Conditional\">" +
                                                     "<TriggerConditional />" +
                                                     "</IntermediateEvent>" +
                                                     "</Event>" +
                                                     "<Documentation />" +
                                                     "<NodeGraphicsInfos>" +
                                                     "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                     "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                     "</NodeGraphicsInfo>" +
                                                     "</NodeGraphicsInfos>" +
                                                     "<ExtendedAttributes />" +
                                                     "</Activity>";
        string EventIntemediateNoneTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                                     "  <Description />" +
                                                     "<Event>" +
                                                     "<IntermediateEvent Trigger=\"None\">" +
                                                     "</IntermediateEvent>" +
                                                     "</Event>" +
                                                     "<Documentation />" +
                                                     "<NodeGraphicsInfos>" +
                                                     "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                     "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                     "</NodeGraphicsInfo>" +
                                                     "</NodeGraphicsInfos>" +
                                                     "<ExtendedAttributes />" +
                                                     "</Activity>";
        string EventIntemediateErrorTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                                     "  <Description />" +
                                                     "<Event>" +
                                                     "<IntermediateEvent Trigger=\"None\">" +
                                                     "</IntermediateEvent>" +
                                                     "</Event>" +
                                                     "<Documentation />" +
                                                     "<NodeGraphicsInfos>" +
                                                     "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                     "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                     "</NodeGraphicsInfo>" +
                                                     "</NodeGraphicsInfos>" +
                                                     "<ExtendedAttributes />" +
                                                     "</Activity>";
        string EventIntemediateCancelTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                                     "  <Description />" +
                                                     "<Event>" +
                                                     "<IntermediateEvent Trigger=\"None\">" +
                                                     "</IntermediateEvent>" +
                                                     "</Event>" +
                                                     "<Documentation />" +
                                                     "<NodeGraphicsInfos>" +
                                                     "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                     "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                     "</NodeGraphicsInfo>" +
                                                     "</NodeGraphicsInfos>" +
                                                     "<ExtendedAttributes />" +
                                                     "</Activity>";
        string EventThrowIntemediateMessageTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                                 "  <Description />" +
                                                 "<Event>" +
                                                 "<IntermediateEvent Trigger=\"Message\">" +
                                                 "<TriggerResultMessage CatchThrow=\"THROW\">" +
                                                 "<Message Id=\"{6}\" />" +
                                                 "</TriggerResultMessage>" +
                                                 "</IntermediateEvent>" +
                                                 "</Event>" +
                                                 "<Documentation />" +
                                                 "<NodeGraphicsInfos>" +
                                                 "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                 "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                 "</NodeGraphicsInfo>" +
                                                 "</NodeGraphicsInfos>" +
                                                 "<ExtendedAttributes />" +
                                                 "</Activity>";
        string EndEventThrowMessageTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                                 "  <Description />" +
                                                 "<Event>" +
                                                 "<EndEvent Result=\"Message\">" +
                                                 "<TriggerResultMessage CatchThrow=\"THROW\">" +
                                                 "<Message Id=\"{6}\" />" +
                                                 "</TriggerResultMessage>" +
                                                 "</EndEvent>" +
                                                 "</Event>" +
                                                 "<Documentation />" +
                                                 "<NodeGraphicsInfos>" +
                                                 "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                 "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                 "</NodeGraphicsInfo>" +
                                                 "</NodeGraphicsInfos>" +
                                                 "<ExtendedAttributes />" +
                                                 "</Activity>";
        string EndEventErrorTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                                 "  <Description />" +
                                                 "<Event>" +
                                                 "<EndEvent Result=\"Error\">" +
                                                 "<ResultError />" +
                                                 "</EndEvent>" +
                                                 "</Event>" +
                                                 "<Documentation />" +
                                                 "<NodeGraphicsInfos>" +
                                                 "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                 "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                 "</NodeGraphicsInfo>" +
                                                 "</NodeGraphicsInfos>" +
                                                 "<ExtendedAttributes />" +
                                                 "</Activity>";
        //
        string EndEventNoneTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                                 "  <Description />" +
                                                 "<Event>" +
                                                 "<EndEvent Result=\"None\">" +
                                                 "</EndEvent>" +
                                                 "</Event>" +
                                                 "<Documentation />" +
                                                 "<NodeGraphicsInfos>" +
                                                 "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                 "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                 "</NodeGraphicsInfo>" +
                                                 "</NodeGraphicsInfos>" +
                                                 "<ExtendedAttributes />" +
                                                 "</Activity>";
        string EndEventSignalTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                                 "  <Description />" +
                                                 "<Event>" +
                                                 "<EndEvent Result=\"Signal\">" +
                                                 "<TriggerResultSignal CatchThrow=\"THROW\"  />" +
                                                 "</EndEvent>" +
                                                 "</Event>" +
                                                 "<Documentation />" +
                                                 "<NodeGraphicsInfos>" +
                                                 "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                 "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                 "</NodeGraphicsInfo>" +
                                                 "</NodeGraphicsInfos>" +
                                                 "<ExtendedAttributes />" +
                                                 "</Activity>";
        string EndEventCancelTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                                 "  <Description />" +
                                                 "<Event>" +
                                                 "<EndEvent Result=\"Cancel\">" +
                                                 "</EndEvent>" +
                                                 "</Event>" +
                                                 "<Documentation />" +
                                                 "<NodeGraphicsInfos>" +
                                                 "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                 "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                 "</NodeGraphicsInfo>" +
                                                 "</NodeGraphicsInfos>" +
                                                 "<ExtendedAttributes />" +
                                                 "</Activity>";
        string EventIntemediateMessageTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                                 "  <Description />" +
                                                 "<Event>" +
                                                 "<IntermediateEvent Trigger=\"Message\">" +
                                                 "<TriggerResultMessage>" +
                                                 "<Message Id=\"{6}\" />" +
                                                 "</TriggerResultMessage>" +
                                                 "</IntermediateEvent>" +
                                                 "</Event>" +
                                                 "<Documentation />" +
                                                 "<NodeGraphicsInfos>" +
                                                 "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                 "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                 "</NodeGraphicsInfo>" +
                                                 "</NodeGraphicsInfos>" +
                                                 "<ExtendedAttributes />" +
                                                 "</Activity>";
        string EventIntemediateMessageNonTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                                 "  <Description />" +
                                                 "<Event>" +
                                                 "<IntermediateEvent Trigger=\"Message\">" +
                                                 "<TriggerResultMessage>" +
                                                 "<Message Id=\"{6}\" />" +
                                                 "</TriggerResultMessage>" +
                                                 "</IntermediateEvent>" +
                                                 "</Event>" +
                                                 "<Documentation />" +
                                                 "<NodeGraphicsInfos>" +
                                                 "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                                 "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                 "</NodeGraphicsInfo>" +
                                                 "</NodeGraphicsInfos>" +
                                                 "<ExtendedAttributes />" +
                                                 "</Activity>";
        string EventConditionalTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                          "  <Description />" +
                                          "<Event>" +
                                          "<StartEvent Trigger=\"Conditional\">" +
                                          "<TriggerConditional />" +
                                          "</StartEvent>" +
                                          "</Event>" +
                                          "<Documentation />" +
                                          "<NodeGraphicsInfos>" +
                                          "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                          "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                          "</NodeGraphicsInfo>" +
                                          "</NodeGraphicsInfos>" +
                                          "<ExtendedAttributes />" +
                                          "</Activity>";
        string EventConditionalNonTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                          "  <Description />" +
                                          "<Event>" +
                                          "<StartEvent Trigger=\"Conditional\">" +
                                          "<TriggerConditional />" +
                                          "</StartEvent>" +
                                          "</Event>" +
                                          "<Documentation />" +
                                          "<NodeGraphicsInfos>" +
                                          "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10311914\" FillColor=\"-1638505\">" +
                                          "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                          "</NodeGraphicsInfo>" +
                                          "</NodeGraphicsInfos>" +
                                          "<ExtendedAttributes />" +
                                          "</Activity>";
        string ExlusiveGetwayTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                        "<Description />" +
                                        "<Route />" +
                                        "<Documentation />" +
                                        "<NodeGraphicsInfos>" +
                                        "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-5855715\" FillColor=\"-52\">" +
                                        "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                        "</NodeGraphicsInfo>" +
                                        "</NodeGraphicsInfos>" +
                                        "<ExtendedAttributes />" +
                                        "</Activity>";
        string InclusiveGetwayTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                         "<Description />" +
                                         "<Route GatewayType=\"Inclusive\" />" +
                                         "<Documentation />" +
                                         "<NodeGraphicsInfos>" +
                                         "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-5855715\" FillColor=\"-52\">" +
                                         "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                         "</NodeGraphicsInfo>" +
                                         "</NodeGraphicsInfos>" +
                                         "<ExtendedAttributes />" +
                                         "</Activity>";

        string ParallelGetwayTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                         "<Description />" +
                                         "<Route GatewayType=\"Parallel\" />" +
                                         "<Documentation />" +
                                         "<NodeGraphicsInfos>" +
                                         "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-5855715\" FillColor=\"-52\">" +
                                         "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                         "</NodeGraphicsInfo>" +
                                         "</NodeGraphicsInfos>" +
                                         "<ExtendedAttributes />" +
                                         "</Activity>";
        string ComplexGetwayTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                         "<Description />" +
                                         "<Route GatewayType=\"Complex\" />" +
                                         "<Documentation />" +
                                         "<NodeGraphicsInfos>" +
                                         "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-5855715\" FillColor=\"-52\">" +
                                         "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                         "</NodeGraphicsInfo>" +
                                         "</NodeGraphicsInfos>" +
                                         "<ExtendedAttributes />" +
                                         "</Activity>";
        string EventBasedGetwayTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                         "<Description />" +
                                         "<Route ExclusiveType=\"Event\" />" +
                                         "<Documentation />" +
                                         "<NodeGraphicsInfos>" +
                                         "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-5855715\" FillColor=\"-52\">" +
                                         "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                         "</NodeGraphicsInfo>" +
                                         "</NodeGraphicsInfos>" +
                                         "<ExtendedAttributes />" +
                                         "</Activity>";
        string ExlusiveEventBasedGetwayTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                         "<Description />" +
                                         "<Route ExclusiveType=\"Event\" Instantiate=\"true\" />" +
                                         "<Documentation />" +
                                         "<NodeGraphicsInfos>" +
                                         "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-5855715\" FillColor=\"-52\">" +
                                         "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                         "</NodeGraphicsInfo>" +
                                         "</NodeGraphicsInfos>" +
                                         "<ExtendedAttributes />" +
                                         "</Activity>";
        string ParallelEventBasedGetwayTemplate = "<Activity Id=\"{0}\" Name=\"{1}\">" +
                                                  "<Description />" +
                                                  "<Route GatewayType=\"Parallel\" Instantiate=\"true\" ParallelEventBased=\"true\"  />" +
                                                  "<Documentation />" +
                                                  "<NodeGraphicsInfos>" +
                                                  "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-5855715\" FillColor=\"-52\">" +
                                                  "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                  "</NodeGraphicsInfo>" +
                                                  "</NodeGraphicsInfos>" +
                                                  "<ExtendedAttributes />" +
                                                  "</Activity>";
        private string ParallelEventBasedIntermediateGetwayTemplate = "<Activity Id=\"{0}\" Name=\"??{1}\">" +
                                                                      "<Description />" +
                                                                      "<Route GatewayType=\"Parallel\" Instantiate=\"true\" ParallelEventBased=\"true\"  />" +
                                                                      "<Documentation />" +
                                                                      "<NodeGraphicsInfos>" +
                                                                      "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-5855715\" FillColor=\"-52\">" +
                                                                      "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                                                      "</NodeGraphicsInfo>" +
                                                                      "</NodeGraphicsInfos>" +
                                                                      "<ExtendedAttributes />" +
                                                                      "</Activity>";

        private string DataObjectTemplate = "<DataObject Id=\"{0}\" Name=\"{1}\" State=\"\">" +
                                            "<Object>" +
                                            "<Documentation />" +
                                            "</Object>" +
                                            "<NodeGraphicsInfos>" +
                                            "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{2}\" Width=\"{3}\" BorderColor=\"-10066330\" FillColor=\"-986896\">" +
                                            "<Coordinates XCoordinate=\"{4}\" YCoordinate=\"{5}\" />" +
                                            "</NodeGraphicsInfo>" +
                                            "</NodeGraphicsInfos>" +
                                            "<DataField />" +
                                            "<ExtendedAttributes />" +
                                            "</DataObject>";

        private string SequenceFlowTemplate = "<Transition Id=\"{0}\" From=\"{1}\" To=\"{2}\">" +
                                              "<Condition />" +
                                              "<Description />" +
                                              "<ConnectorGraphicsInfos>" +
                                              "<ConnectorGraphicsInfo ToolId=\"BizAgi_Process_Modeler\" BorderColor=\"0\">" +
                                              "{3}" +
                                              "</ConnectorGraphicsInfo>" +
                                              "</ConnectorGraphicsInfos>" +
                                              "<ExtendedAttributes />" +
                                              "</Transition>";

        private string MessageFlowTemplate = "<MessageFlow Id=\"{0}\" Source=\"{1}\" Target=\"{2}\">" +
                                             "<ConnectorGraphicsInfos>" +
                                             "<ConnectorGraphicsInfo ToolId=\"BizAgi_Process_Modeler\" BorderColor=\"0\">" +
                                             "{3}" +
                                             "</ConnectorGraphicsInfo>" +
                                             "</ConnectorGraphicsInfos>" +
                                             "<ExtendedAttributes />" +
                                             "</MessageFlow>";

        string CoordinatesTemplate = "<Coordinates XCoordinate=\"{0}\" YCoordinate=\"{1}\" />";

        private string DataAssociationTemplate = "<DataAssociation Id=\"{0}\" From=\"{1}\" To=\"{2}\">" +
                                                 "<Description />" +
                                                 "<ConnectorGraphicsInfos>" +
                                                 "<ConnectorGraphicsInfo ToolId=\"BizAgi_Process_Modeler\" BorderColor=\"-16777216\" />" +
                                                 "</ConnectorGraphicsInfos>" +
                                                 "<ExtendedAttributes />" +
                                                 "</DataAssociation>";

        string OutputSetTemplate = "<OutputSet>" +
                                   "<Output ArtifactId=\"{0}\" />" +
                                   "</OutputSet>";

        string InputSetTemplate = "<InputSet>" +
                                  "<Input ArtifactId=\"{0}\" />" +
                                  "</InputSet>";

        string DataInputTemplate = "<DataInput Id=\"{0}\" IsCollection=\"false\">" +
                                   "<NodeGraphicsInfos>" +
                                   "<NodeGraphicsInfo BorderVisible=\"false\" ToolId=\"BizAgi_Process_Modeler\" Height=\"50\" Width=\"40\" BorderColor=\"-10066330\" FillColor=\"-986896\">" +
                                   "<Coordinates XCoordinate=\"0\" YCoordinate=\"0\" />" +
                                   "</NodeGraphicsInfo>" +
                                   "</NodeGraphicsInfos>" +
                                   "<Documentation />" +
                                   "</DataInput>";

        string DataOutputTemplate = "<DataOutput Id=\"{0}\" IsCollection=\"false\">" +
                                    "<NodeGraphicsInfos>" +
                                    "<NodeGraphicsInfo BorderVisible=\"false\" ToolId=\"BizAgi_Process_Modeler\" Height=\"50\" Width=\"40\" BorderColor=\"-10066330\" FillColor=\"-986896\">" +
                                    "<Coordinates XCoordinate=\"0\" YCoordinate=\"0\" />" +
                                    "</NodeGraphicsInfo>" +
                                    "</NodeGraphicsInfos>" +
                                    "<Documentation />" +
                                    "</DataOutput>";

        private string AssociationTemplate = "<Association Id=\"{0}\" Source=\"{1}\" Target=\"{2}\">" +
                                             "<ConnectorGraphicsInfos>" +
                                             "<ConnectorGraphicsInfo ToolId=\"BizAgi_Process_Modeler\" BorderColor=\"0\">" +
                                             "{3}" +
                                             "</ConnectorGraphicsInfo>" +
                                             "</ConnectorGraphicsInfos>" +
                                             "<ExtendedAttributes />" +
                                             "</Association>";

        private string PolTemplate = "<Pool Id=\"{0}\" Name=\"{1}\" Process=\"{2}\" BoundaryVisible=\"true\">" +
                                     "<Lanes>" +
                                     "{3}" +
                                     "</Lanes>" +
                                     "<NodeGraphicsInfos>" +
                                     "<NodeGraphicsInfo BorderVisible=\"false\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{4}\" Width=\"{5}\" BorderColor=\"-16777216\" FillColor=\"-1\">" +
                                     "<Coordinates XCoordinate=\"{6}\" YCoordinate=\"{7}\" />" +
                                     "</NodeGraphicsInfo>" +
                                     "</NodeGraphicsInfos>" +
                                     "</Pool>";

        private string LaneTemplate = "<Lane Id=\"{0}\" Name=\"{1}\" ParentPool=\"{2}\">" +
                                      "<NodeGraphicsInfos>" +
                                      "<NodeGraphicsInfo BorderVisible=\"true\" ToolId=\"BizAgi_Process_Modeler\" Height=\"{3}\" Width=\"{4}\" BorderColor=\"-11513776\" FillColor=\"-1\">" +
                                      "<Coordinates XCoordinate=\"50\" YCoordinate=\"0\" />" +
                                      "</NodeGraphicsInfo>" +
                                      "</NodeGraphicsInfos>" +
                                      "<ExtendedAttributes />" +
                                      "</Lane>";

    }
}
