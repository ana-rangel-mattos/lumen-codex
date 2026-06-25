import * as React from "react";
import { Button } from "../index.ts";

interface IDialogProps {
  dialogTitle: string;
  content: string;
  onConfirm: () => void;
  confirmActionText?: string;
  isOpen?: boolean;
  setIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
}

const Dialog: React.FC<IDialogProps> = (props) => {
  const {
    dialogTitle,
    content,
    onConfirm,
    confirmActionText = "Confirm",
    isOpen = false,
    setIsOpen,
  } = props;

  return (
    <div
      className={`${isOpen ? "" : "hidden"} bg-base-content fixed top-30 right-30 left-30 z-20 space-y-4 rounded-md px-6 py-4 lg:inset-65`}
    >
      <p className="font-800 text-base-100 text-2xl">{dialogTitle}</p>
      <p className="text-base-300 text-lg">{content}</p>
      <div className="mt-6 flex items-center justify-around">
        <Button
          size="sm"
          buttonStyle="secondary"
          onClick={() => setIsOpen(false)}
        >
          Cancel
        </Button>
        <Button
          buttonStyle="warning"
          size="sm"
          className={`!text-primary-content`}
          onClick={onConfirm}
        >
          {confirmActionText}
        </Button>
      </div>
    </div>
  );
};

export default Dialog;
