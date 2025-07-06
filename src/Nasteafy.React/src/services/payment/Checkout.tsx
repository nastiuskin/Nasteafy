import { useState } from "react";
import {
  PayPalButtons,
  usePayPalScriptReducer,
} from "@paypal/react-paypal-js";

type CheckoutProps = {
  amount: number;
  currency?: string;
  onSuccess?: (payerName: string) => void;
};

export default function Checkout({
  amount,
  currency = "USD",
  onSuccess,
}: CheckoutProps) {
  const [{ isPending }] = usePayPalScriptReducer();
  const [selectedCurrency, setSelectedCurrency] = useState(currency);

  const onCurrencyChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedCurrency(e.target.value);
  };

  const onCreateOrder = (_data: any, actions: any) => {
    return actions.order.create({
      purchase_units: [
        {
          amount: {
            value: amount.toFixed(2),
          },
        },
      ],
    });
  };

  const onApproveOrder = (_data: any, actions: any) => {
    return actions.order.capture().then((details: any) => {
      const name = details.payer?.name?.given_name || "User";
      onSuccess?.(name);
    });
  };

  return (
    <div className="checkout space-y-4">
      {isPending ? (
        <p className="text-muted-foreground">Loading PayPal...</p>
      ) : (
        <>
          <select
            value={selectedCurrency}
            onChange={onCurrencyChange}
            className="bg-muted border rounded px-2 py-1 text-sm"
          >
            <option value="USD">USD</option>
            <option value="EUR">EUR</option>
          </select>

          <PayPalButtons
            key={selectedCurrency}
            style={{ layout: "vertical" }}
            createOrder={onCreateOrder}
            onApprove={onApproveOrder}
          />
        </>
      )}
    </div>
  );
}