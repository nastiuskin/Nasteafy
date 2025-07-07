import { useEffect, useState } from "react";
import { client } from "../../api/ApiClientProvider";
import { handleApiError } from "../../helpers/handleApiError";
import { SubscribeUserCommand, type GetSubscriptionDto } from "../../api/apiClient";
import { Card } from "../../components/ui/card";
import { Button } from "../../components/ui/button";
import { Badge } from "../../components/ui/badge";
import { useAuth } from "../../hooks/useAuth";
import Checkout from "../../services/payment/Checkout";
import toast from "react-hot-toast";
import { authService } from "../../services/auth/AuthService";

export default function SubscriptionsPage() {
  const [subscriptions, setSubscriptions] = useState<GetSubscriptionDto[]>([]);
  const [selectedId, setSelectedId] = useState<string | null>(null);
  const { user, setUser } = useAuth();

  useEffect(() => {
    const fetchSubscriptions = async () => {
      try {
        const result = await client.subscriptionsGET();
        setSubscriptions(result.subscriptions ?? []);
      } catch (err) {
        handleApiError(err);
      }
    };

    fetchSubscriptions();
  }, []);


 const subscribe = async (subscriptionId: string, subscriptionName: string) => {
  try {
    const request = new SubscribeUserCommand({ subscriptionId });
    await client.subscribe(request);
    setSelectedId(null);

    setUser({
      ... user!,
      subscriptionType: subscriptionName,
    });

    toast.success("Subscription successfully activated");
    window.location.reload();
  } catch (err) {
    handleApiError(err);
  }
};

  const isValidAmount = (val?: number): val is number => typeof val === "number" && val > 0;

  return (
    <div className="p-6 max-w-3xl mx-auto space-y-6">
      <h1 className="text-3xl font-bold">Available Subscriptions</h1>

      {subscriptions.length === 0 ? (
        <p className="text-muted-foreground">No subscriptions found.</p>
      ) : (
        subscriptions.map((sub) => {
          const isActive = sub.name === user?.subscriptionType;
          const isSelected = sub.id === selectedId;

          return (
            <Card
              key={sub.id}
              className={`shadow-md border rounded-2xl p-4 ${isActive ? "border-green-500" : "border-border"}`}
            >
              <div className="flex items-start justify-between">
                <div className="space-y-1">
                  <div className="flex items-center gap-2">
                    <h2 className="text-lg font-semibold text-foreground">{sub.name}</h2>
                    {isActive && (
                      <Badge variant="success">Active</Badge>
                    )}
                  </div>
                  <p className="text-sm text-muted-foreground">{sub.description}</p>
                </div>
                <div className="text-primary font-bold text-base whitespace-nowrap">
                  {sub.price?.toFixed(2)} $
                </div>
              </div>

              {!isActive && (
                <div className="pt-4 flex justify-end">
                  <Button
                    onClick={() => setSelectedId(sub.id!)}
                    className="px-6 py-1.5 text-sm rounded-full"
                  >
                    Subscribe
                  </Button>
                </div>
              )}

              {isSelected && isValidAmount(sub.price) && (
                <div className="mt-6 border-t pt-6">
                  <h2 className="text-xl font-semibold mb-2">
                    Complete Payment for {sub.name}
                  </h2>

                  <Checkout
                    amount={sub.price}
                    onSuccess={() => subscribe(sub.id!, sub.name!)}
                  />
                </div>
              )}
            </Card>
          );
        })
      )}
    </div>
  );
}
