namespace MentalHealthcare.API.Docs;

public static class OrderProcessingDocs
{
    public const string AddToCartDescription = """
                                               ### Add to Cart
                                               Adds a course to the user's cart.

                                               #### Request:
                                               - **CourseId** (Required): The ID of the course to be added to the cart.

                                               #### Response:
                                               - Returns an integer representing the cart ID.

                                               #### Edge Cases:
                                               - If the user is not authorized, an error will be thrown.
                                               - If the course is already in the cart, an error will be returned.
                                               - If the course does not exist, an error will be thrown.
                                               - If the cart has reached its item limit, an error will be thrown.

                                               #### Example Usage:
                                               To add a course to the cart:
                                               ```json
                                               POST /api/cart/add
                                               {
                                                 "courseId": 123
                                               }
                                               ```
                                               """;

    public const string DeleteFromCartDescription = """
                                                    ### Delete From Cart
                                                    Removes a specific course from the user's cart.

                                                    #### Request:
                                                    - **CourseId** (Required): The ID of the course to be removed from the cart.

                                                    #### Response:
                                                    - No content is returned upon success.

                                                    #### Edge Cases:
                                                    - If the user is not authorized, an error will be thrown.
                                                    - If the course is not found in the cart, no action will be taken.

                                                    #### Example Usage:
                                                    To remove a course from the cart:
                                                    ```json
                                                    DELETE /api/cart/delete
                                                    {
                                                      "courseId": 123
                                                    }
                                                    ```
                                                    """;

    public const string ClearCartDescription = """
                                               ### Clear Cart
                                               Clears all items from the user's cart.

                                               #### Request:
                                               - No parameters are required.

                                               #### Response:
                                               - No content is returned upon success.

                                               #### Edge Cases:
                                               - If the user is not authorized, an error will be thrown.
                                               - If the cart is already empty, no action will be taken.

                                               #### Example Usage:
                                               To clear all items from the cart:
                                               ```http
                                               DELETE /api/cart/clear
                                               ```
                                               """;

    public const string GetCartItemsDescription = """
                                                  ### Get Cart Items
                                                  Retrieves the items in the current user's cart, calculates totals, discounts, and taxes. A promo code can be optionally applied to adjust the pricing.

                                                  #### Request:
                                                  - **PromoCode** (Optional): A promo code string to apply discounts to the cart items.

                                                  #### Response:
                                                  - Returns a `CartDto` object containing:
                                                    - **Courses**: A list of items in the cart with their details.
                                                    - **NumberOfItems**: Total number of items in the cart.
                                                    - **SubTotalPrice**: The total price before applying discounts and taxes.
                                                    - **DiscountValue**: The value of the discount applied.
                                                    - **DiscountPercentage**: The discount percentage.
                                                    - **TaxesValue**: The calculated tax value (rounded to the nearest 0.5).
                                                    - **TaxesPercentage**: The tax percentage applied.
                                                    - **TotalPrice**: The final calculated total after applying discounts and taxes (rounded to the nearest 0.5).
                                                    - **Messages**: A list of messages indicating the status of the promo code or any adjustments applied. If the promo code is invalid, this field will contain a descriptive message, but the endpoint will still process the request.

                                                  #### Example Usage:
                                                  To retrieve the cart with an optional promo code:
                                                  ```http
                                                  GET /api/cart?PromoCode=SAVE10
                                                  ```

                                                  If no promo code is provided:
                                                  ```http
                                                  GET /api/cart
                                                  ```
                                                  """;

    public const string GetInvoiceDescription = """
                                                ### Get Invoice
                                                Retrieves an invoice by its ID. The caller must be authenticated with a valid Bearer token.

                                                #### Role-Based Behavior:
                                                - **Admin**: Can retrieve any invoice regardless of ownership.
                                                - **User**: Can only retrieve invoices that belong to them.

                                                #### Response:
                                                - Returns an `InvoiceDto` object containing:
                                                  - **InvoiceId**: The unique ID of the invoice.
                                                  - **UserId**: The ID of the user associated with the invoice.
                                                  - **FirstName**: First name of the user.
                                                  - **LastName**: Last name of the user.
                                                  - **PhoneNumber**: Phone number of the user.
                                                  - **Email**: Email address of the user.
                                                  - **OrderStatus**: Current status of the order.
                                                  - **Courses**: A list of courses associated with the invoice.
                                                  - **TotalPrice**: The total price of the invoice.
                                                  - **DiscountPercentage**: The applied discount percentage.
                                                  - **SubTotalPrice**: Price before taxes and discounts.
                                                  - **NumberOfItems**: Total items in the invoice.
                                                  - **TaxesValue**: Calculated tax value.
                                                  - **TaxesPercentage**: Applied tax percentage.
                                                  - **OrderDate**: Date when the order was created.
                                                  - **PromoCode**: Promo code applied to the order, if any.
                                                  - **DiscountValue**: Value of the applied discount.

                                                #### Edge Cases:
                                                - If the invoice does not exist, a 404 error is returned.
                                                - If the user is not authorized, a 401 error is returned.
                                                - If the user does not have permission to access the invoice, a 404 error is returned.

                                                #### Example Usage:
                                                To retrieve an invoice by its ID:
                                                ```http
                                                GET /api/invoice/123
                                                Authorization: Bearer <token>
                                                ```
                                                """;

    public const string GetAllInvoicesDescription = """

                                                    ### Get All Invoices
                                                    Retrieves a paginated list of invoices based on query parameters. The endpoint is authorized and requires the caller to provide a valid Bearer token.

                                                    #### Role-Based Behavior:
                                                    - **Admin**: Can retrieve all invoices.
                                                    - **User**: Can only retrieve their own invoices.

                                                    #### Request Parameters:
                                                    - **PageNumber**: The page number for pagination (default: 1).
                                                    - **PageSize**: The number of items per page (default: 10).
                                                    - **Status** (Optional): Filters invoices by their status:
                                                        - **Pending** (0): Awaiting processing.
                                                        - **Done** (1): Completed invoices.
                                                        - **Rejected** (2): Declined invoices.
                                                        - **Expired** (3): No longer valid.
                                                    - **Name** (Optional): Filters by the user's name (partial match).
                                                    - **Email** (Optional): Filters by the user's email (partial match).
                                                    - **PhoneNumber** (Optional): Filters by the user's phone number (partial match).
                                                    - **FromDate** (Optional): Filters invoices created on or after this date. Format: `yyyy-MM-ddTHH:mm:ss` (e.g., `2023-12-01T00:00:00`).
                                                    - **ToDate** (Optional): Filters invoices created on or before this date. Format: `yyyy-MM-ddTHH:mm:ss` (e.g., `2023-12-31T23:59:59`).
                                                    - **PromoCode** (Optional): Filters invoices with the specified promo code.

                                                    #### Status Values:
                                                    - **Pending** (0): The invoice is awaiting processing.
                                                    - **Done** (1): The invoice has been successfully completed.
                                                    - **Rejected** (2): The invoice has been declined or rejected.
                                                    - **Expired** (3): The invoice is no longer valid due to expiration.

                                                    #### Responses:
                                                    - **200 OK**: Returns a paginated list of `InvoiceViewDto`.
                                                    - **401 Unauthorized**: If the caller is not authenticated.
                                                    - **403 Forbidden**: If the caller lacks permission.

                                                    #### Example Usage:
                                                    ```http
                                                    GET /api/invoices?pageNumber=1&pageSize=10&status=Pending&fromDate=2023-12-01T00:00:00&toDate=2023-12-31T23:59:59
                                                    Authorization: Bearer <token>
                                                    ```
                                                    """;

    public const string CalculateInvoiceDescription = """
                                                      Calculates the value of an invoice based on the provided courses, discount percentage, and tax percentage.

                                                      #### When to Use:
                                                      - When an admin wants to change the price of some courses or apply a larger discount to the entire invoice and evaluate the results.
                                                      - This API is purely for recalculating the invoice and does not perform any actions, such as saving changes or verifying prices.

                                                      #### Constraints:
                                                      - All provided course IDs must be unique.
                                                      - The API does not validate or enforce price constraints.
                                                      - The user must be authorized with a valid Bearer token.

                                                      #### Warning:
                                                      - This API does not save or take any action on the provided data. It is intended for calculation purposes only.

                                                      #### Example Usage:
                                                      ```http
                                                      POST /CalculateInvoiceValue
                                                      Authorization: Bearer <token>
                                                      Content-Type: application/json

                                                      {
                                                        "Courses": [
                                                          { "CourseId": 1, "Price": 100.0 },
                                                          { "CourseId": 2, "Price": 200.0 }
                                                        ],
                                                        "DiscountPercentage": 10.0,
                                                        "TaxPercentage": 15.0
                                                      }
                                                      ```

                                                      #### Response:
                                                      - Returns a recalculated invoice with updated prices, discounts, taxes, and total values.
                                                      """;

    public const string AcceptInvoiceDescription = """
                                                   ### Accept Invoice
                                                   This API allows an admin to accept an invoice, potentially modify the discount, and update the courses in the invoice. The caller must be authenticated with a valid Bearer token.

                                                   #### Request Parameters:
                                                   - **Path Parameter**:
                                                     - `invoiceId` (int, required): The ID of the invoice to accept.
                                                   - **Request Body**:
                                                     - `AcceptInvoiceCommand` (required):
                                                       - **Courses**: A list of courses in the invoice, each with its ID and price. You can pass fewer courses than originally in the invoice (indicating removal), but you cannot pass courses that were not part of the original invoice.
                                                       - **Discount**: The new discount value to apply to the invoice.

                                                   #### Constraints:
                                                   - Courses passed must already exist in the original invoice.
                                                   - Courses not included in the request will be removed from the invoice.
                                                   - The admin can override the discount value by passing a new one.

                                                   #### Response:
                                                   - **204 No Content**: If the operation is successful.
                                                   - **400 Bad Request**: If any invalid course IDs are passed or the invoice is not in a pending state.
                                                   - **401 Unauthorized**: If the caller is not authenticated.
                                                   #### Example Usage:
                                                   ```http
                                                   POST /api/invoices/123/accept
                                                   Authorization: Bearer <token>
                                                   Content-Type: application/json

                                                   {
                                                     "Courses": [
                                                       { "CourseId": 1, "Price": 100.0 },
                                                       { "CourseId": 2, "Price": 200.0 }
                                                     ],
                                                     "Discount": 10.0
                                                   }
                                                   ```

                                                   #### Notes:
                                                   - If the discount passed differs from the original invoice, it will be updated with the new value.
                                                   - Any course not passed in the request will be removed from the invoice.
                                                   - The API ensures that all passed course IDs are valid and belong to the original invoice.
                                                   """;

    public const string ChangeInvoiceStateDescription = """
                                                        Changes the state of an invoice to a specified order status.

                                                        #### Allowed Transitions:
                                                        - **To Expired**: Can only change if the current status is `Pending`.
                                                        - **To Pending**: Can only change if the current status is `Expired` or `Cancelled`.
                                                        - **To Cancelled**: Can only change if the current status is `Pending`.

                                                        #### Request Parameters:
                                                        - **Path Parameter**:
                                                          - `invoiceId` (int, required): The ID of the invoice to update.
                                                        - **Request Body**:
                                                          - `status` (OrderStatus, required): The target status to which the invoice should be updated.

                                                        #### OrderStatus Enum Values:
                                                        - `0` - Pending
                                                        - `1` - Done (not allowed through this API)
                                                        - `2` - Cancelled
                                                        - `3` - Expired

                                                        #### Constraints:
                                                        - If the provided `status` is invalid or does not exist in the `OrderStatus` enum, an exception will be thrown.
                                                        - Transitioning to `Done` is not allowed through this API.

                                                        #### Response:
                                                        - **204 No Content**: If the state transition is successful.
                                                        - **400 Bad Request**: If the provided status is invalid or the transition is not allowed.
                                                        - **404 Not Found**: If the invoice does not exist.
                                                        - **500 Internal Server Error**: For unexpected errors.

                                                        #### Example Usage:
                                                        ```http
                                                        PATCH /api/invoices/123/state
                                                        Content-Type: application/json

                                                        {
                                                          "status": 2
                                                        }
                                                        ```

                                                        #### Notes:
                                                        - The API validates the state transitions based on the current invoice status.
                                                        - Ensure the correct `OrderStatus` value is passed as part of the request.
                                                        """;
}