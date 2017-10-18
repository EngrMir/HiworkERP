

namespace HiWork.Utils
{
    public enum EstimationType
    {
        Translation = 1,
        Interpreter = 2,
        SchoolExcursion = 3,
        ShortTermDispatch = 4,
        Project = 5,
        DTP = 6,
        Narration = 7,
        TapeOver = 8,
        WebCreation = 9,
        Transcription = 10,
        OverheadCost = 11
    }

    public enum ApplicationType
    {
        ERP = 1,
        TransPro = 2,
        EditingPro = 3,
        TransProJapan = 4,
        TransproKorea = 5,
        TransproAus =6        
    }

    public enum EstimationStatus
    {
        Under_estimation = 1,
        Waiting_for_approval = 2,
        Approved = 3,
        Waiting_for_confirmation = 4,
        Ordered = 5,
        Under_arrangement = 6,
        In_progress = 7,
        Delivered = 8,
        Invoice_completed = 9,
        Waiting_for_deposit = 10,
        Not_deposited = 11,
        Order_Completed = 12,
        Complaint = 13,
        Cancel = 14,
        Deleted = 15
    }
    public enum OrderStatus
    {
        UnderEstimation = 1,
        WaitingForApproval = 2,
        WaitingForConfirmation = 3,
        Ordered = 4,
        UnderArrangement = 5,
        InProgress = 6,
        Delivered = 7,
        InvoiceCompleted = 8,
        WaitingForDeposit = 9,
        NotDeposited = 10,
        OrderCompleted = 11,
        Complaint = 12,
        Cancel = 13,
        Deleted = 14
    }

    public enum EstimationApprovalStatus
    {
        Read = 1,
        Unread = 2,
        Clicked = 3,
        Approved = 4,
    }

    public enum CertificateType
    {
        IssuedByTranslator = 1,
        IssuedByCompany = 2
    }

    public enum MessageStatus
    {
        Sent=1,
        Unread=2,
        Read=3,
        Replied=4
    }
    public enum CompanyRegistrationType
    {
        Individual=1,
        LegalEntity=2,
        Public=3
    }
    public enum TranslationType
    {
        Online = 1,
        Appointed = 2,
        NativeCheck = 3,
        TranslatorCoordinator=4
    }

    public enum WebOrderStatus
    {
        WaitingForConfirmation = 1,
        Arranging = 2,
        Ordered = 3,
        WaitingForStaffConfirmation = 4,
        InProgress = 5,
        Delivered = 6,
        Evaluated = 7,
        WaitingForDeposit = 8,
        OrderCompleted = 9,
        Complaint = 10,
        Cancel = 11,
        Deleted = 12
    }

    public enum StaffType
    {
        Translator=1,
        Narrator=2,
        Interpretor=3,
        Coordinator=4,
        RealTime_STAFF=5,
    }
    public enum PaymentMethod
    {
        CreditCard=1,
        MonthlyPayment=2,
        RequestedPayment=3        
    }
    public enum PaymentStatus
    {
        Unpaid=0,
        Paid=1
    }

    public enum DocumentDataCountType
    {
        CharacterCount = 1,
        WordCount = 2,
        MixedCount = 2
    }
}
