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
  onSuccess,
}: CheckoutProps) {
  const [{ isPending }] = usePayPalScriptReducer();

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
          <PayPalButtons
            style={{ layout: "vertical" }}
            createOrder={onCreateOrder}
            onApprove={onApproveOrder}
          />
        </>
      )}
    </div>
  );
}