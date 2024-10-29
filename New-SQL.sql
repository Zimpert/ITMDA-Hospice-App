CREATE TABLE `Users` (
  `UserID` varchar(36),
  `Role` enum('Admin', 'Caregiver', 'Patient'),
  `Name` varchar(255),
  `Surname` varchar(255),
  `Phone` varchar(255),
  `Email` varchar(255),
  `Address` varchar(255),
  `PasswordHash` varchar(255),
  PRIMARY KEY (`UserID`)
);


CREATE TABLE `AssignedPatients` (
  `AssignmentID` varchar(36) PRIMARY KEY,
  `CaregiverID` varchar(36),
  `PatientID` varchar(36),
  FOREIGN KEY (`CaregiverID`) REFERENCES `Users`(`UserID`),
  FOREIGN KEY (`PatientID`) REFERENCES `Users`(`UserID`)
);


CREATE TABLE `Conditions` (
  `ConditionID` varchar(36),
  `ConditionName` varchar(255),
  `Description` varchar(255),
  PRIMARY KEY (`ConditionID`)
);


CREATE TABLE `Medications` (
  `MedicationID` varchar(36),
  `MedicationName` varchar(255),
  `Description` varchar(255),
  `Interactions` varchar(255),
  PRIMARY KEY (`MedicationID`)
);
CREATE TABLE `Reminders` (
  `ReminderID` varchar(36),
  `Type` enum('medication', 'appointment', 'task'),
  `Description` varchar(255),
  `Completed` bool,
  `Priority` enum('Low', 'Medium', 'High'),
  PRIMARY KEY (`ReminderID`)
);



CREATE TABLE `PatientMedications` (
  `PatientMedicationID` varchar(36) PRIMARY KEY,
  `PatientID` varchar(36),
  `MedicationID` varchar(36),
  FOREIGN KEY (`MedicationID`) REFERENCES `Medications`(`MedicationID`),
  FOREIGN KEY (`PatientID`) REFERENCES `Users`(`UserID`)
);

CREATE TABLE `PatientMedicationSchedule` (
  `PatientMedicationScheduleID` varchar(36),
  `PatientMedicationID` varchar(36),
  PRIMARY KEY (`PatientMedicationScheduleID`),
  FOREIGN KEY (`PatientMedicationID`) REFERENCES `PatientMedications` (`PatientMedicationID`)
);



CREATE TABLE `MedicationDays` (
  `PatientMedicationDayID` varchar(36),
  `PatientMedicationScheduleID` Varchar(36),
  `Day` enum('Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'),
  `ReminderID` varchar(36),
  `Frequency` enum('Hourly', '2 Hourly', '3 Hourly', '4 Hourly', '6 Hourly', '8 Hourly', '12 Hourly'),
  PRIMARY KEY (`PatientMedicationDayID`),
FOREIGN KEY (`PatientMedicationScheduleID`) REFERENCES `PatientMedicationSchedule` (`PatientMedicationScheduleID`),
  FOREIGN KEY (`ReminderID`) REFERENCES `Reminders`(`ReminderID`)

);



CREATE TABLE `PatientConditions` (
  `PatientConditionID` varchar(36),
  `PatientID` varchar(36),
  `ConditionID` varchar(36),
  PRIMARY KEY (`PatientConditionID`),
  FOREIGN KEY (`ConditionID`) REFERENCES `Conditions`(`ConditionID`),
  FOREIGN KEY (`PatientID`) REFERENCES `Users`(`UserID`)
);


CREATE TABLE `PatientAppointments` (
  `PatientAppointmentID` varchar(36),
  `PatientID` varchar(36),
  `AppointmentName` varchar(255),
  `Description` varchar(255),
  `ReminderID` varchar(36),
  `AppointmentDate` DATE,
  `AppoinmentStart` DateTime,
  `AppointmentEnd` Datetime,
  PRIMARY KEY (`PatientAppointmentID`),
  FOREIGN KEY (`PatientID`) REFERENCES `Users`(`UserID`),
  FOREIGN KEY (`ReminderID`) REFERENCES `Reminders`(`ReminderID`)
);


CREATE TABLE `ContactBooks` (
  `ContactBookID` varchar(36),
  `PatientID` varchar(36),
  PRIMARY KEY (`ContactBookID`),
  FOREIGN KEY (`PatientID`) REFERENCES `Users`(`UserID`),
);

CREATE TABLE `PersonalContacts` (
  `PersonalContactID` varchar(36),
  `ContactBookID` varchar(36),
  `PersonalContactName` varchar(255),
  `PersonalContactSurname` varchar(255),
  `Phone` varchar(255),
  `Email` varchar(255),
  PRIMARY KEY (`PersonalContactID`),
  FOREIGN KEY (`ContactBookID`) REFERENCES `ContactBook`(`ContactBookID`)
);


CREATE TABLE `MedicalContacts` (
  `MedicalContactID` varchar(36),
  `MedicalContactName` varchar(255),
  `MedicalContactSurname` varchar(255),
  `Phone` varchar(255),
  `Email` varchar(255),
  PRIMARY KEY (`MedicalContactID`)
);


CREATE TABLE `PatientMedecalContacts` (
  `PatientMedecalContactID` varchar(36),
  `ContactBookID` varchar(36),
  `MedicalContactID` varchar(36),
  PRIMARY KEY (`PatientMedecalContactID`),
  FOREIGN KEY (`MedicalContactID`) REFERENCES `MedicalContacts`(`MedicalContactID`),
  FOREIGN KEY (`ContactBookID`) REFERENCES `ContactBook`(`ContactBookID`)
);



CREATE TABLE `CaregiverShifts` (
  `CaregiverShiftID` varchar(36),
  `CaregiverID` varchar(36),
  `PatientID` varchar(36),
  `ReminderID` varchar(36),
  PRIMARY KEY (`CaregiverShiftID`),
  FOREIGN KEY (`PatientID`) REFERENCES `Users`(`UserID`),
FOREIGN KEY (`CaregiverID`) REFERENCES `Users`(`UserID`),
FOREIGN KEY (`ReminderID`) REFERENCES `Reminders`(`ReminderID`)
);


CREATE TABLE `Logs` (
  `LogID` varchar(36),
  `LogAction` enum('Clock-In', 'Clock-Out'),
  `LogDate` DateTime,
  `CaregiverID` varchar(36),
  `PatientID` varchar(36),
  PRIMARY KEY (`LogID`),
  FOREIGN KEY (`PatientID`) REFERENCES `Users`(`UserID`),
FOREIGN KEY (`CaregiverID`) REFERENCES `Users`(`UserID`)
);


















