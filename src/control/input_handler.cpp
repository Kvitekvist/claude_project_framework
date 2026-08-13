// Simple input handler - captures keyboard/mouse and sends to driver via shared memory
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <iostream>
#include <cmath>

#define SHARED_MEMORY_NAME L"VREmulator_Input"
#define SHARED_MEMORY_SIZE 1024

struct SharedInputData {
    // HMD pose
    float hmd_pos_x, hmd_pos_y, hmd_pos_z;
    float hmd_yaw, hmd_pitch, hmd_roll;

    // Controller states
    bool left_trigger;
    bool left_grip;
    bool right_trigger;
    bool right_grip;

    bool running;
};

int main() {
    std::cout << "VREmulator Input Handler Starting..." << std::endl;
    std::cout << "Controls:" << std::endl;
    std::cout << "  Mouse        - Look around" << std::endl;
    std::cout << "  W/A/S/D      - Move forward/left/back/right" << std::endl;
    std::cout << "  Space/Ctrl   - Move up/down" << std::endl;
    std::cout << "  Left Click   - Right trigger" << std::endl;
    std::cout << "  Right Click  - Right grip" << std::endl;
    std::cout << "  ESC          - Exit" << std::endl;
    std::cout << std::endl;

    // Create shared memory
    HANDLE hMapFile = CreateFileMappingW(
        INVALID_HANDLE_VALUE,
        NULL,
        PAGE_READWRITE,
        0,
        SHARED_MEMORY_SIZE,
        SHARED_MEMORY_NAME
    );

    if (hMapFile == NULL) {
        std::cerr << "Could not create file mapping object (" << GetLastError() << ")" << std::endl;
        return 1;
    }

    SharedInputData* pData = (SharedInputData*)MapViewOfFile(
        hMapFile,
        FILE_MAP_ALL_ACCESS,
        0,
        0,
        SHARED_MEMORY_SIZE
    );

    if (pData == NULL) {
        std::cerr << "Could not map view of file (" << GetLastError() << ")" << std::endl;
        CloseHandle(hMapFile);
        return 1;
    }

    // Initialize
    pData->hmd_pos_x = 0.0f;
    pData->hmd_pos_y = 1.6f;  // Standing height
    pData->hmd_pos_z = 0.0f;
    pData->hmd_yaw = 0.0f;
    pData->hmd_pitch = 0.0f;
    pData->hmd_roll = 0.0f;
    pData->left_trigger = false;
    pData->left_grip = false;
    pData->right_trigger = false;
    pData->right_grip = false;
    pData->running = true;

    std::cout << "Shared memory created. Driver should connect..." << std::endl;

    // Input loop
    POINT lastMousePos;
    GetCursorPos(&lastMousePos);

    const float MOUSE_SENSITIVITY = 0.002f;
    const float MOVE_SPEED = 0.05f;

    while (pData->running) {
        // Check for ESC to exit
        if (GetAsyncKeyState(VK_ESCAPE) & 0x8000) {
            std::cout << "ESC pressed - exiting..." << std::endl;
            break;
        }

        // Mouse look
        POINT currentMousePos;
        GetCursorPos(&currentMousePos);

        int deltaX = currentMousePos.x - lastMousePos.x;
        int deltaY = currentMousePos.y - lastMousePos.y;

        if (GetAsyncKeyState(VK_RBUTTON) & 0x8000) {  // Right mouse held for look
            pData->hmd_yaw -= deltaX * MOUSE_SENSITIVITY;
            pData->hmd_pitch -= deltaY * MOUSE_SENSITIVITY;

            // Clamp pitch
            if (pData->hmd_pitch > 1.5f) pData->hmd_pitch = 1.5f;
            if (pData->hmd_pitch < -1.5f) pData->hmd_pitch = -1.5f;
        }

        lastMousePos = currentMousePos;

        // Movement (WASD)
        float moveX = 0, moveY = 0, moveZ = 0;

        if (GetAsyncKeyState('W') & 0x8000) {
            moveZ -= MOVE_SPEED * cosf(pData->hmd_yaw);
            moveX -= MOVE_SPEED * sinf(pData->hmd_yaw);
        }
        if (GetAsyncKeyState('S') & 0x8000) {
            moveZ += MOVE_SPEED * cosf(pData->hmd_yaw);
            moveX += MOVE_SPEED * sinf(pData->hmd_yaw);
        }
        if (GetAsyncKeyState('A') & 0x8000) {
            moveZ -= MOVE_SPEED * sinf(pData->hmd_yaw);
            moveX += MOVE_SPEED * cosf(pData->hmd_yaw);
        }
        if (GetAsyncKeyState('D') & 0x8000) {
            moveZ += MOVE_SPEED * sinf(pData->hmd_yaw);
            moveX -= MOVE_SPEED * cosf(pData->hmd_yaw);
        }
        if (GetAsyncKeyState(VK_SPACE) & 0x8000) {
            moveY += MOVE_SPEED;
        }
        if (GetAsyncKeyState(VK_CONTROL) & 0x8000) {
            moveY -= MOVE_SPEED;
        }

        pData->hmd_pos_x += moveX;
        pData->hmd_pos_y += moveY;
        pData->hmd_pos_z += moveZ;

        // Controller buttons
        pData->right_trigger = (GetAsyncKeyState(VK_LBUTTON) & 0x8000) != 0;
        pData->right_grip = (GetAsyncKeyState(VK_MBUTTON) & 0x8000) != 0;
        pData->left_trigger = (GetAsyncKeyState(VK_XBUTTON1) & 0x8000) != 0;
        pData->left_grip = (GetAsyncKeyState(VK_SHIFT) & 0x8000) != 0;

        Sleep(16);  // ~60 FPS
    }

    // Cleanup
    UnmapViewOfFile(pData);
    CloseHandle(hMapFile);

    std::cout << "Input handler stopped." << std::endl;
    return 0;
}
