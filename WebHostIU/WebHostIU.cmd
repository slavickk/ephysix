set CURRENT_DIR=%CD%
set DATA_ROOT_DIR=%CURRENT_DIR%/../Pubs/
set MOCKED=Step_finishpayment_AnsSign_PUPAY;Step_Rollback_finishpayment_Endpay;Step_finishpayment_MONEY_PUPAY;Step_step_0_STEP_OLD;Step_step_0_STEP;Step_export_0_EXPORT;Step_finishpayment_0_PUPAY;Step_requestotp_0_Sign;Step_requestotp_1_Sign;Step_requestotp_2_Sign;Step_pupay_0_PUPAY;Step_finishpayment_Sign_PUPAY;Step_finishpayment_EndPay_PUPAY
set YAML_PATH=%CURRENT_DIR%/../Pubs/Pipeline/patt1.yml
set SIGN_SERVICE__SIGN__ADDRESS=http://212.233.113.20:8070/mb-sign-doc/ws/sign
set SIGN_SERVICE__VERIFY__ADDRESS=http://212.233.113.20:8070/mb-sign-doc/ws/verify
WebHostIU.exe